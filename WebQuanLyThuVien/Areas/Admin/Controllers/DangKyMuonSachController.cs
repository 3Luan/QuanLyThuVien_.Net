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
    public class DangKyMuonSachController : Controller
    {
        DangKyMuonSachService _dangKyMuonSachService = new DangKyMuonSachService();
        PhieuMuonCTPhieuMuonService _phieuMuonCTPhieuMuonService = new PhieuMuonCTPhieuMuonService();

        // GET: Admin/DangKyMuonSach
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
                var dkiMuonSach = _dangKyMuonSachService.GetAllDangKyMuonSach();
                ViewData["dkiMuonSach"] = dkiMuonSach;
                return View();
            }
        }


        [HttpGet]
        public ActionResult GetAllDkiMuonSach()
        {
            var dkiMuonSach = _dangKyMuonSachService.GetAllDangKyMuonSach();
            return Json(new ApiOkResponse(dkiMuonSach.ToList()), JsonRequestBehavior.AllowGet);
        }



        [HttpPost]
        public ActionResult GetDKSachMuonByMaDK(int maDK)
        {
            var data = _dangKyMuonSachService.Get_DangKySachMuon_ByMaDK(maDK);

            // Trả về dữ liệu JSON
            if (data != null)
            {
                return Json(new { success = true, data = data });
            }
            else
            {
                return Json(new { success = false });
            }
        }



        [HttpPost]
        public ActionResult GetChiTietDKByMaDK(int maDK)
        {
            var data = _dangKyMuonSachService.Get_CTDK_ByMaDK(maDK);

            if (data != null)
            {
                // Trả về dữ liệu JSON
                return Json(new { success = true, data = data });
            }
            else
            {
                return Json(new { success = false });
            }

        }

        [HttpPost]
        public ActionResult CheckDocGia(string SDT)
        {
            try
            {
                var data = _dangKyMuonSachService.CheckDocGia(SDT);

                if (data)
                {
                    return Json(new { success = true, message = "SDT đã đăng ký thẻ", data = data });
                }
                else
                {
                    return Json(new { success = true, message = "SDT chưa đăng ký thẻ", data = data });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "An error occurred while checking DocGia" });
            }
        }

        [HttpPost]
        public ActionResult HandleBtnHuyAndDuyet(int maDK, int tinhTrang)
        {
            try
            {
                var data = _dangKyMuonSachService.UpdateTinhTrang(maDK, tinhTrang);

                if (data)
                {
                    return Json(new { success = true, message = "Update tình trạng thành công", data = data });
                }
                else
                {
                    return Json(new { success = true, message = "Update tình trạng thất bại", data = data });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "error HandleBtnHuyDon: ", ex });
            }
        }


        [HttpPost]
        public ActionResult SubmitTaoPhieuMuon(int maNV, int maDK, DateTime ngayTra, DateTime ngayMuon, string sdt)
        {
            try
            {
                var danhSachDK = _dangKyMuonSachService.Get_CTDK_ByMaDK(maDK);
                var maThe = _dangKyMuonSachService.GetMaTheBySDT(sdt);

                var checkHanThe = _dangKyMuonSachService.CheckHanTheDocGia(maDK);
                if (checkHanThe)
                {
                    DTO_Tao_Phieu_Muon tpm = new DTO_Tao_Phieu_Muon();
                    tpm.NgayMuon = ngayMuon;
                    tpm.NgayTra = ngayTra;
                    tpm.MaNhanVien = maNV;
                    tpm.MaTheDocGia = maThe;
                    tpm.MaDK = maDK;
                    tpm.listSachMuon = danhSachDK;

                    _phieuMuonCTPhieuMuonService.Insert(tpm);
                    return Json(new { success = true, data = tpm });
                }
                else
                {
                    return Json(new { success = true, message = "Thẻ độc giả đã hết hạn", data = "checkthedocgia" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "error HandleBtnHuyDon: ", ex });
            }
        }
    }
}