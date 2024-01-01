using Microsoft.Ajax.Utilities;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Web;
using System.Web.Mvc;
//using WebQuanLyThuVien.App_Start;
using WebQuanLyThuVien.Models;
using PagedList;

namespace ThuVienBTL.Controllers
{
    public class ClassesController : Controller
    {
        QuanLyThuVienEntities db = new QuanLyThuVienEntities();

        // GET: Classess    
        public ActionResult Index(int? page)
        {

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