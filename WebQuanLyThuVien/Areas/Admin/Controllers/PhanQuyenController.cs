using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebQuanLyThuVien.Areas.Admin.Data;
using WebQuanLyThuVien.Interfaces.Services;
using WebQuanLyThuVien.Models;
using WebQuanLyThuVien.Services;

namespace WebQuanLyThuVien.Areas.Admin.Controllers
{
    public class PhanQuyenController : Controller
    {
        NhanVienService _nhanVienService = new NhanVienService();


        // GET: Admin/PhanQuyen
        public ActionResult Index()
        {
            if (Session["user"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else if (!(Session["chucvu"].ToString().ToLower() == "admin"))
            {
                return RedirectToAction("loiphanquyen", "phanquyen");
            }
            else
            {
                var thongTinNhanVien = _nhanVienService.GetAll();
                ViewData["ThongTinNhanVien"] = thongTinNhanVien;

                return View();
            }
        }

        public ActionResult LoiPhanQuyen()
        {
            if (Session["user"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                return View();
            }
        }


        [HttpGet]
        public ActionResult GetAllNhanVien()
        {
            try
            {
                var nhanVien = _nhanVienService.GetAllNhanVien();

                return Json(new { Result = nhanVien }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                // Trong trường hợp có lỗi, có thể log lỗi hoặc xử lý theo nhu cầu của bạn
                return Json(new { Error = "Không thể lấy dữ liệu nhan vien." });
            }
        }



        [HttpPost]
        public ActionResult ThemNhanVien(string hoTen, string soDienThoai, string gioiTinh, DateTime ngaySinh, string diaChi, string chucVu, string username, string password)
        {
            try
            {
                DTO_NhanVien_LoginNV nv = new DTO_NhanVien_LoginNV();

                nv.HoTenNV = hoTen;
                nv.SDT = soDienThoai;
                nv.GioiTinh = gioiTinh;
                nv.NgaySinh = ngaySinh;
                nv.DiaChi = diaChi;
                nv.ChucVu = chucVu;
                nv.Username = username;
                nv.Password = password;

                _nhanVienService.ThemNhanVien(nv);

                return Json(new { success = true, data = nv });

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

        [HttpPost]
        public ActionResult ThongTinNhanVien(int id)
        {
            try
            {
                var nhanVien = _nhanVienService.GetById(id);

                if (nhanVien != null)
                {
                    // Trả về dữ liệu JSON nếu thành công
                    return Json(new { success = true, data = nhanVien });
                }
                else
                {
                    // Trả về thông báo lỗi nếu không tìm thấy sách
                    return Json(new { success = false, message = "Không tìm thấy nhân viên" });
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
        public ActionResult CapNhatThongTin(int maNV, DateTime ngaySinh, string diaChi, string gioiTinh, string soDienThoai, string hoTen, string chucVu, string username, string password)
        {
            try
            {
                DTO_NhanVien_LoginNV nv = new DTO_NhanVien_LoginNV();

                nv.MaNV = maNV;
                nv.NgaySinh = ngaySinh;
                nv.DiaChi = diaChi;
                nv.GioiTinh = gioiTinh;
                nv.SDT = soDienThoai;
                nv.HoTenNV = hoTen;
                nv.ChucVu = chucVu;
                nv.Username = username;
                nv.Password = password;

                _nhanVienService.UpdateThongTinNhanVien(nv);

                return Json(new { success = true, data = nv });
            }
            catch (Exception ex)
            {
                // Kiểm tra xem lỗi có phải do tồn tại số điện thoại không
                if (ex.Message.Contains("Số điện thoại đã tồn tại."))
                    return Json(new { success = false, message = "Số điện thoại đã tồn tại." });

                // Kiểm tra xem lỗi có phải do tồn tại số username không
                if (ex.Message.Contains("Username đã tồn tại."))
                    return Json(new { success = false, message = "Username đã tồn tại." });

                // Xử lý lỗi nếu có
                Console.WriteLine($"Error: {ex.Message}");
                return Json(new { success = false, message = "Đã xảy ra lỗi" });
            }
        }
    }
}