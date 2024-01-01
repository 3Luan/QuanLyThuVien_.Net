using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebQuanLyThuVien.Areas.Admin.Data;
using WebQuanLyThuVien.Areas.Admin.Interfaces.Services;
using WebQuanLyThuVien.Areas.Admin.Services;
using WebQuanLyThuVien.Models;
using WebQuanLyThuVien.Services;
using System.Transactions;

namespace WebQuanLyThuVien.Areas.Admin.Controllers
{


    public class PhieuTraController : Controller
    {
        // GET: Admin/PhieuTra
        PhieuMuonService _phieuMuonService = new PhieuMuonService();

        PhieuMuonService _sachMuonService = new PhieuMuonService();
        PhieuTraCTPhieuTraService _phieuTraCTPhieuTraService = new PhieuTraCTPhieuTraService();
        // GET: Admin/PhieuMuon
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
                var phieuMuon = _phieuMuonService.GetPhieuMuonsChuaTraSach();
                ViewData["PhieuMuon"] = phieuMuon;
                return View();
            }
        }


        [HttpGet]
        public JsonResult phieuMuon(string keyword)
        {
            if (keyword == "")
            {
                var phieuMuon = _phieuMuonService.GetPhieuMuonsChuaTraSach();
                return Json(new ApiOkResponse(phieuMuon.ToList()), JsonRequestBehavior.AllowGet);
            }
            else
            {
                var phieuMuon = _phieuMuonService.SearchPhieuMuon(keyword);
                return Json(new ApiOkResponse(phieuMuon.ToList()), JsonRequestBehavior.AllowGet);
            }

        }
        [HttpPost]
        public ActionResult GetListPhieuMuonPaging(GetListPhieuMuonPaging req)
        {
            var phieuMuon = _phieuMuonService.GetAllPhieuMuonPaging(req);
            if (phieuMuon != null)
            {
                return Json(new ApiOkResponse(phieuMuon));
            }
            return Json(new ApiNotFoundResponse(""));
        }

        [HttpGet]
        public JsonResult GetSachMuon(int maPM)
        {
            if (maPM != 0)
            {
                // Nếu phieumuon tồn tại, tiếp tục thực hiện các thao tác khác
                var sachMuon = _sachMuonService.getSachMuon(maPM); // Thay thế bằng phương thức thực tế để lấy dữ liệu sách mượn
                return Json(new ApiOkResponse(sachMuon.ToList()), JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new ApiNotFoundResponse(""), JsonRequestBehavior.AllowGet);
            }
        }


        [HttpPost]
        public ActionResult TaoPhieuTra(DTO_Tao_Phieu_Tra data)
        {
            try
            {
                var success = _phieuTraCTPhieuTraService.Insert(data);
                if (success)
                { // Trả về phản hồi thành công
                  // var phieuMuon = _phieuMuonService.GetPhieuMuonsChuaTraSach();
                    return Json(new { success = true, message = "Tạo phiếu trả thành công." });
                }
                return Json(new { success = false, message = "Tạo phiếu trả thất bại." });
            }
            catch (Exception ex)
            {
                // Xử lý các ngoại lệ một cách thích hợp
                return Json(new { success = false, message = "Lỗi xử lý yêu cầu.", error = ex.Message });
            }

        }

    }




}

