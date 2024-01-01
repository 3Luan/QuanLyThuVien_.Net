using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebQuanLyThuVien.Areas.Admin.Services;
using WebQuanLyThuVien.Models;
using WebQuanLyThuVien.Services;

namespace WebQuanLyThuVien.Areas.Admin.Controllers
{
    public class HomeController : Controller
    {
        DocGiaService docGiaService = new DocGiaService();
        SachService sachService = new SachService();
        NhapSachService nhapSachService = new NhapSachService();
        PhieuThanhLyService phieuThanhLyService = new PhieuThanhLyService();
        PhieuTraCTPhieuTraService phieuTraCTPhieuTraService = new PhieuTraCTPhieuTraService();

        // GET: Admin/Home
        public ActionResult Index()
        {
            if (Session["user"] == null)
                return RedirectToAction("Login", "Account");
            else
            {
                var theDocGia = docGiaService.GetAllTheDocGia();
                var sach = sachService.GetAll();
                var chiTietPN = nhapSachService.GetAllChiTietPhieuNhap();
                var phieuThanhLy = phieuThanhLyService.GetAllSachTL();
                var chiTietPT = phieuTraCTPhieuTraService.GetAllChiTietPT();

                decimal tongTienDangKyThe = 0;
                var soLuongDocGia = 0;
                var soLuongSach = 0;
                decimal tongTienNhapSach = 0;
                decimal tongTienThanhLySach = 0;
                decimal tongTienPhuThu = 0;
                decimal doanhThu = 0;

                // Thẻ độc giả
                foreach (var item in theDocGia)
                {
                    tongTienDangKyThe = tongTienDangKyThe + item.TienThe;
                    soLuongDocGia += 1;
                }

                // sách
                foreach (var item in sach)
                {
                    soLuongSach += item.SoLuongHIENTAI.Value;
                }

                // Chi tiết phiếu nhập
                foreach (var item in chiTietPN)
                {
                    tongTienNhapSach += item.GiaSach.Value * item.SoLuongNHAP.Value;
                }

                // Phiếu thanh lý
                foreach (var item in phieuThanhLy)
                {
                    tongTienThanhLySach += item.GiaSachTL * item.SoLuongKhoTL;
                }

                // Chi tiết phiếu trả
                foreach (var item in chiTietPT)
                {
                    tongTienPhuThu += item.PhuThu.Value;
                }

                doanhThu = tongTienDangKyThe + tongTienThanhLySach + tongTienPhuThu;

                ViewData["tongTienDangKyThe"] = tongTienDangKyThe;
                ViewData["soLuongDocGia"] = soLuongDocGia;
                ViewData["soLuongSach"] = soLuongSach;
                ViewData["tongTienNhapSach"] = tongTienNhapSach;
                ViewData["tongTienThanhLySach"] = tongTienThanhLySach;
                ViewData["tongTienPhuThu"] = tongTienPhuThu;
                ViewData["doanhThu"] = doanhThu;

                return View();
            }
        }
    }
}