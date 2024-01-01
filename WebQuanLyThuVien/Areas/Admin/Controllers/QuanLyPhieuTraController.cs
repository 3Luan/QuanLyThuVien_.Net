using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebQuanLyThuVien.Areas.Admin.Data;
using WebQuanLyThuVien.Areas.Admin.Interfaces.Services;
using WebQuanLyThuVien.Areas.Admin.Services;

namespace WebQuanLyThuVien.Areas.Admin.Controllers
{
    public class QuanLyPhieuTraController : Controller
    {
        // GET: Admin/QuanLyPhieuTra
        PhieuTraService _phieuTraService = new PhieuTraService();

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
                var phieuTra = _phieuTraService.GetAllPhieuTra();
                ViewData["PhieuTra"] = phieuTra;
                return View();
            }
        }
        [HttpPost]
        public ActionResult ChiTietPhieuTra(int id)
        {
            var listCTPT = _phieuTraService.Get_CTPT_ByID(id);
            //ViewData["CTPhieuMuon"] = ctpm;

            // Trả về dữ liệu JSON
            return Json(new { success = true, data = listCTPT });
        }

        [HttpPost]
        public ActionResult ChiTietptByPM(int Mapm)
        {
            var listCTPT = _phieuTraService.Get_ChiTietPT_ByMaPM(Mapm);
            //ViewData["CTPhieuMuon"] = ctpm;

            // Trả về dữ liệu JSON
            return Json(new { success = true, data = listCTPT });
        }

        [HttpPost]
        public ActionResult GetListPhieuTraPaging(GetListPhieuTraPaging req)
        {
            var phieuTra = _phieuTraService.GetAllPhieuTraPaging(req);
            if (phieuTra != null)
            {
                return Json(new ApiOkResponse(phieuTra));
            }
            return Json(new ApiNotFoundResponse(""));
        }
    }
}