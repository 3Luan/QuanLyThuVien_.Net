using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls; 
using WebQuanLyThuVien.App_Start;
using WebQuanLyThuVien.Models;
using System.Text.RegularExpressions;
using PagedList;

namespace WebQuanLyThuVien.Controllers
{
    public class ThueSachController : Controller
    {
        // GET: ThueSach
        QuanLyThuVienEntities db = new QuanLyThuVienEntities();

        public ActionResult Index(int? page)
        {
            // Kiem tra quyen su dung
            var user = SessionConfig.GetUser();
            if (user == null)
            {
                return RedirectToAction("Index", "User");
            }

            // Kích thước trang (số lượng mục trên mỗi trang)
            int pageSize = 9;

            // Số trang hiện tại (mặc định là 1 nếu không có giá trị)
            int pageNumber = (page ?? 1);

            // Lấy danh sách sách từ cơ sở dữ liệu
            List<Sach> listSach = db.Saches.ToList();

            // Sử dụng PagedList để chia danh sách thành các trang
            IPagedList<Sach> pagedListSach = listSach.ToPagedList(pageNumber, pageSize);

            // Truyền thông tin phân trang vào ViewBag để sử dụng trong View
            ViewBag.CurrentPage = pageNumber;
            ViewBag.TotalPages = pagedListSach.PageCount;

            return View(pagedListSach);
        }

        [HttpPost]
        public ActionResult LocYeuCau(string NgonNgu, string TheLoai, string NamXB)
        {
            List<Sach> sachLocNgonNgu;
            List<Sach> sachLocTheLoai;
            List<Sach> sachLocNamXB;

            // Lọc theo ngôn ngữ
            if (NgonNgu.ToString() != "All")
            {
                // nếu có chọn lọc ngôn ngữ thì tiến hành lọc
                sachLocNgonNgu = db.Saches.Where(m => m.NgonNgu == NgonNgu).ToList();
            }
            else
            {
                sachLocNgonNgu = db.Saches.ToList();
            }

            // Lọc theo thể loại
            if (TheLoai.ToString() != "All")
            {
                sachLocTheLoai = sachLocNgonNgu.Where(m => m.TheLoai == TheLoai).ToList();
            }
            else
            {
                sachLocTheLoai = sachLocNgonNgu;
            }

            // Lọc theo năm xuất bản
            if (NamXB.ToString() != "All")
            {
                int number = int.Parse(NamXB);
                sachLocNamXB = sachLocTheLoai.Where(m => m.NamXB == number).ToList();
            }
            else
            {
                sachLocNamXB = sachLocTheLoai;
            }

            var maSach = sachLocNamXB.Select(sach => sach.MaSach).ToList();

            if (maSach.Count > 0)
            {
                return Json(new { success = true, sachList = maSach });
            }
            else
            {
                return Json(new { success = false, message = "Không tìm thấy sách hoặc sách không tồn tại." });
            }
        }

        [HttpPost]
        public ActionResult SearchSach(string search)
        {
            if (string.IsNullOrEmpty(search))
            {
                return Json(new { success = false, message = "Không tìm thấy sách hoặc sách không tồn tại." });
            }
            else
            {
                try
                {
                    // Tìm kiếm theo yêu cầu người dùng
                    List<Sach> sachLoc = db.Saches.Where(item => item.TenSach.Contains(search)
                    || item.TheLoai.Contains(search) || item.TacGia.Contains(search) || item.NgonNgu.Contains(search)
                    || item.NXB.Contains(search)).ToList();

                    // Đưa toàn bộ mã sách tìm được vào một mảng lưu trữ mã sách
                    var maSach = sachLoc.Select(sach => sach.MaSach).ToList();

                    if (maSach.Count > 0)
                    {
                        return Json(new { success = true, sachList = maSach });
                    }
                    else
                    {
                        return Json(new { success = false, message = "Không tìm thấy sách hoặc sách không tồn tại." });
                    }
                }
                catch
                {
                    return Json(new { success = false, message = "Không tìm thấy sách hoặc sách không tồn tại." });
                }
            }
        }

        // Tạo đối tượng để trả về view tìm kiếm
        public class SachDetailsResponse
        {
            public Sach SachDetails { get; set; }
            public string UrlImage { get; set; }
        }

        [HttpPost]
        public ActionResult GetSachDetails(int maSach)
        {
            var sachChon = db.Saches.FirstOrDefault(s => s.MaSach == maSach);

            Sach sachDetails = new Sach()
            {
                MaSach = sachChon.MaSach,
                NamXB = sachChon.NamXB,
                NgonNgu = sachChon.NgonNgu,
                NXB = sachChon.NXB,
                SoLuongHIENTAI = sachChon.SoLuongHIENTAI,
                TacGia = sachChon.TacGia,
                TenSach = sachChon.TenSach,
                TheLoai = sachChon.TheLoai,
            };

            var urlImage = db.TT_SACH.Find(maSach).URL_IMAGE;

            // Tạo đối tượng chứa thông tin cần trả về
            var response = new SachDetailsResponse
            {
                SachDetails = sachDetails,
                UrlImage = urlImage
            };

            // Trả về đối tượng dưới dạng JSON
            return Json(response);
        }

        public ActionResult GioHang()
        {
            return View();
        }


        [HttpPost]
        public ActionResult DangKyMuon(int maSach, int soLuongMuon)
        {
            try
            {
                // kiểm tra nếu sách đã được thêm vào thì cập nhật số lượng sách bằng tổng số sách 2 lần nhập
                if (ListSachMuon.listSachMuon.ContainsKey(maSach))
                {
                    var value = ListSachMuon.listSachMuon[maSach];   // lấy ra số sách khách hàng đã mượn trước đó
                    ListSachMuon.listSachMuon[maSach] = value + soLuongMuon;   // cập nhật tổng số sách
                }
                else
                {
                    // Nếu sách chưa được nhập trước đó thì thêm sách mới
                    Sach sach = db.Saches.Find(maSach);
                    ListSachMuon.listSachMuon.Add(maSach, soLuongMuon);
                }

                return Json(new { success = true }); // Trả về JSON để xử lý trong script nếu cần
            }
            catch (Exception ex)
            {
                return Json(new { success = false, error = ex.Message });
            }
        }

        [HttpPost]
        public ActionResult XoaSachMuon(int maSach)
        {
            try
            {
                if (ListSachMuon.listSachMuon.ContainsKey(maSach))  // Kiểm tra mã sách đưa vào có tồn tại hay không
                {
                    ListSachMuon.listSachMuon.Remove(maSach);
                    return Json(new { success = true, message = "Cập nhật số lượng thành công" });
                }
                else
                {
                    return Json(new { success = false, message = "Không tìm thấy sách." });
                }
            }
            catch
            {
                return Json(new { success = false, message = "Đã xảy ra lỗi khi xoá sách." });
            }
        }

        [HttpPost]
        public ActionResult TinhTongSoSachMuon()
        {
            return Json(new { success = true, tongSoSach = ListSachMuon.listSachMuon.Values.Sum() });
        }

        [HttpPost]
        public ActionResult XacNhanThueSach(int[] maSach, int[] soLuongSach)
        {
            DateTime now = DateTime.Now;

            DkiMuonSach dkMuon = new DkiMuonSach()
            {
                SDT = (string)Session["shared_SDT"],
                NgayDKMuon = now,
                NgayHen = now.AddDays(7),
                Tinhtrang = 0,
            };

            db.DkiMuonSaches.Add(dkMuon);
            // Luu vao database
            db.SaveChanges();

            // Lặp qua từng phần tử của hai mảng cùng một chỉ số
            for (int i = 0; i < Math.Min(maSach.Length, soLuongSach.Length); i++)
            {
                // Lấy ra phần tử tương ứng từ mảng maSach và soLuongSach
                int maSachItem = maSach[i];
                int soLuongSachItem = soLuongSach[i];

                // Tạo một đối tượng mới và thêm vào danh sách
                ChiTietDk ctdk = new ChiTietDk()
                {
                    MaDK = dkMuon.MaDK,
                    MaSach = maSachItem,
                    Soluongmuon = soLuongSachItem,
                };

                db.ChiTietDks.Add(ctdk);

                // Xoá dữ liệu ở model listSachMuon
                ListSachMuon.listSachMuon.Remove(ctdk.MaSach);

            }

            // Luu vào database
            db.SaveChanges();

            return Json(new { success = true });

        }

        [HttpPost]
        public ActionResult DetailsInfo(int maSach)
        {
            var sachInfo = db.Saches.FirstOrDefault(a => a.MaSach == maSach);
            if (sachInfo != null)
            {
                //return View("ThongTinSach", sachInfo);
                return Json(new { success = true, message = "có sách.", data = sachInfo });
            }
            else
            {
                return HttpNotFound();
            }
        }

        public ActionResult ThongTinSach(int id)
        {
            var sachInfo = db.Saches.FirstOrDefault(a => a.MaSach == id);
            return View(sachInfo);
        }
    }
}