using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebQuanLyThuVien.Areas.Admin.Interfaces.Services;
using WebQuanLyThuVien.Areas.Admin.Services;

namespace WebQuanLyThuVien.Areas.Admin.Controllers
{
    public class QuanLyMuonController : Controller
    {
        PhieuMuonService _phieuMuonService = new PhieuMuonService();
        PhieuMuonCTPhieuMuonService _phieuMuonCTPhieuMuonService = new PhieuMuonCTPhieuMuonService();

        // GET: Admin/QuanLyMuon
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
                var phieuMuon = _phieuMuonService.GetPhieuMuonsDocGia();
                ViewData["PhieuMuon"] = phieuMuon;
                return View();
            }
        }

        [HttpPost]
        public ActionResult ChiTietPhieuMuon(int id)
        {
            var listCTPM = _phieuMuonCTPhieuMuonService.Get_CTPM_ByID(id);
            //ViewData["CTPhieuMuon"] = ctpm;

            // Trả về dữ liệu JSON
            return Json(new { success = true, data = listCTPM });
        }
    }
}