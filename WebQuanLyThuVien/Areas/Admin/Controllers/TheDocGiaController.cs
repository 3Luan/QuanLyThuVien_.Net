using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebQuanLyThuVien.Areas.Admin.Data;
using WebQuanLyThuVien.Areas.Admin.Interfaces.Services;
using WebQuanLyThuVien.Areas.Admin.Services;
using WebQuanLyThuVien.Models;

namespace WebQuanLyThuVien.Areas.Admin.Controllers
{
    public class TheDocGiaController : Controller
    {
        DocGiaService _theDocGiaService = new DocGiaService();


        // GET: Admin/TheDocGia
        public ActionResult Index()
        {
            if (Session["user"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else if (Session["chucvu"].ToString().ToLower() == "quanlykho")
            {
                return RedirectToAction("loiphanquyen", "phanquyen");
            }
            else
            {
                var theDocGia = _theDocGiaService.GetAllTheDocGia();

                ViewData["TheDocGia"] = theDocGia;

                return View();
            }
        }
        


        [HttpGet]
        public ActionResult GetAllTheDocGia()
        {
            var theDocGia = _theDocGiaService.GetAllTheDocGia();
            return Json(new ApiOkResponse(theDocGia.ToList()), JsonRequestBehavior.AllowGet);
        }



        [HttpPost]
        public ActionResult ThongTinTheDocGia(int id)
        {
            try
            {
                var theDocGia = _theDocGiaService.GetById(id);

                if (theDocGia != null)
                {
                    // Trả về dữ liệu JSON nếu thành công
                    return Json(new { success = true, data = theDocGia });
                }
                else
                {
                    // Trả về thông báo lỗi nếu không tìm thấy sách
                    return Json(new { success = false, message = "Không tìm thấy độc  giả" });
                }
            }
            catch (Exception ex)
            {
                // Xử lý lỗi nếu có
                Console.WriteLine($"Error in ChiTietDocGia: {ex.Message}");
                return Json(new { success = false, message = "Đã xảy ra lỗi" });
            }
        }

        [HttpPost]
        public ActionResult GiaHanTheDocGia(int maThe, DateTime thoiGianGiaHan, int tienGiaHan)
        {
            try
            {
                TheDocGia tdg = new TheDocGia();

                tdg.MaThe = maThe;
                tdg.NgayHH = thoiGianGiaHan;
                tdg.TienThe = tienGiaHan;

                _theDocGiaService.Update(tdg);

                return Json(new { success = true, data = tdg });

            }
            catch (Exception ex)
            {
                // Xử lý lỗi nếu có
                Console.WriteLine($"Error: {ex.Message}");
                return Json(new { success = false, message = "Đã xảy ra lỗi" });
            }
        }


        [HttpPost]
        public ActionResult DangKyTheDocGia(int maNV, DateTime ngayDK, string tenDocGia, string soDienThoai, string gioiTinh, DateTime ngaySinh, string diaChi, int hanThe, int tienDK)
        {
            try
            {
                DTO_DocGia_TheDocGia tdg = new DTO_DocGia_TheDocGia();

                tdg.MaNhanVien = maNV;
                tdg.NgayDangKy = DateTime.Now;
                tdg.HoTenDG = tenDocGia;
                tdg.SDT = soDienThoai;
                tdg.GioiTinh = gioiTinh;
                tdg.NgaySinh = ngaySinh;
                tdg.DiaChi = diaChi;
                tdg.TienThe = tienDK;
                tdg.NgayHetHan = DateTime.Now.AddMonths(hanThe);

                _theDocGiaService.DangKyTheDocGia(tdg);

                return Json(new { success = true, data = tdg });

            }
            catch (Exception ex)
            {
                // Kiểm tra xem lỗi có phải do tồn tại số điện thoại không
                if (ex.Message.Contains("Số điện thoại đã tồn tại."))
                    return Json(new { success = false, message = "Số điện thoại đã tồn tại." });

                // Nếu không phải là lỗi tồn tại số điện thoại, xử lý lỗi khác
                return Json(new { success = false, message = ex.Message });
            }
        }
    }
}