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
    public class ThongTinDocGiaController : Controller
    {
        // GET: Admin/ThongTinDocGia
        DocGiaService _docGiaService = new DocGiaService();


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
                var thongTinDocGia = _docGiaService.GetAllTheDocGia();
                ViewData["ThongTinDocGia"] = thongTinDocGia;
                return View();
            }
        }

        //[HttpGet]
        //public ActionResult GetAllThongTinDocGia()
        //{
        //    try
        //    {
        //        var thongTinDocGia = _docGiaService.GetAllTheDocGia();

        //        return Json(new { Result = thongTinDocGia }, JsonRequestBehavior.AllowGet);
        //    }
        //    catch (Exception ex) 
        //    {
        //        // Trong trường hợp có lỗi, có thể log lỗi hoặc xử lý theo nhu cầu của bạn
        //        return Json(new { Error = "Không thể lấy dữ liệu ncc." });
        //    }
        //}

        [HttpGet]
        public ActionResult GetAllThongTinDocGia()
        {
            var thongTinDocGia = _docGiaService.GetAllTheDocGia();
            return Json(new ApiOkResponse(thongTinDocGia.ToList()), JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public ActionResult ThongTinTheDocGia(int id)
        {
            try
            {
                var theDocGia = _docGiaService.GetById(id);

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
        public ActionResult CapNhatThongTin(int maDocGia, DateTime ngaySinh, string diaChi, string gioiTinh, string soDienThoai, string tenDocGia)
        {
            try
            {
                DocGia dg = new DocGia();

                dg.MaDG = maDocGia;
                dg.HoTenDG = tenDocGia;
                dg.NgaySinh = ngaySinh;
                dg.DiaChi = diaChi;
                dg.GioiTinh = gioiTinh;
                dg.SDT = soDienThoai;

                _docGiaService.UpdateThongTinDocGia(dg);

                return Json(new { success = true, data = dg });
            }
            catch (Exception ex)
            {
                // Kiểm tra xem lỗi có phải do tồn tại số điện thoại không
                if (ex.Message.Contains("Số điện thoại đã tồn tại."))
                    return Json(new { success = false, message = "Số điện thoại đã tồn tại." });

                // Xử lý lỗi nếu có
                Console.WriteLine($"Error: {ex.Message}");
                return Json(new { success = false, message = "Đã xảy ra lỗi" });
            }
        }
    }
}