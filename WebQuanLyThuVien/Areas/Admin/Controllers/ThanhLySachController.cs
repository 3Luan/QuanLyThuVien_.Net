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
    public class ThanhLySachController : Controller
    {
        // GET: Admin/ThanhLySach
        PhieuThanhLyService _phieuThanhLyService = new PhieuThanhLyService();
        DonViTLService _donViTLService = new DonViTLService();
        KhoThanhLyService _khoThanhLyService = new KhoThanhLyService();

        public ActionResult Index()
        {
            if (Session["user"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else if (Session["chucvu"].ToString().ToLower() == "thuthu")
            {
                return RedirectToAction("loiphanquyen", "phanquyen");
            }
            else
            {
                var phieuTL = _phieuThanhLyService.GetAllDonViTL();
                ViewData["PhieuTL"] = phieuTL;
                return View();
            }
        }

        [HttpGet]
        public JsonResult SelectDV(string keyword)
        {
            if (keyword == "")
            {
                var SelectDV = _phieuThanhLyService.GetAllDonViTL();
                return Json(new ApiOkResponse(SelectDV.ToList()), JsonRequestBehavior.AllowGet);
            }
            else
            {
                var SelectDV = _phieuThanhLyService.SearchDonVi(keyword);
                return Json(new ApiOkResponse(SelectDV.ToList()), JsonRequestBehavior.AllowGet);
            }

        }

        [HttpGet]
        public JsonResult SelectSach(string keyword)
        {
            if (keyword == "")
            {
                var SelectSach = _phieuThanhLyService.GetAllSachTL();
                return Json(new ApiOkResponse(SelectSach.ToList()), JsonRequestBehavior.AllowGet);
            }
            else
            {
                var SelectSach = _phieuThanhLyService.SearchSach(keyword);
                return Json(new ApiOkResponse(SelectSach.ToList()), JsonRequestBehavior.AllowGet);
            }

        }

        [HttpPost]
        public ActionResult TaoSachThanhLy(DTO_Tao_Phieu_TL data)
        {
            try
            {
                var success = _phieuThanhLyService.InsertSachThanhLy(data);
                if (success)
                { // Trả về phản hồi thành công

                    return Json(new { success = true, message = "Tạo Sách thanh lý thành công." });

                }
                return Json(new { success = false, message = "Tạo Sách thanh lý thất bại." });
            }
            catch (Exception ex)
            {
                // Xử lý các ngoại lệ một cách thích hợp
                return Json(new { success = false, message = "Lỗi xử lý yêu cầu.", error = ex.Message });
            }

        }
        [HttpPost]
        public ActionResult ThemDonViThanhLy(string tenDv, string sdtDv, string diaChiDv)
        {
            try
            {
                DonViTL dv = new DonViTL();

                dv.TenDV = tenDv;
                dv.SDTDV = sdtDv;
                dv.DiaChiDV = diaChiDv;

                var success = _donViTLService.Insert(dv);
                if (success)
                {
                    return Json(new { success = true, message = "Thêm đơn vị thành công." });
                }
                return Json(new { success = false, message = "Thêm đơn vị thất bại." });
            }
            catch (Exception ex)
            {
                // Xử lý các ngoại lệ một cách thích hợp
                // Kiểm tra xem lỗi có phải do tồn tại số điện thoại không
                if (ex.Message.Contains("existingDonVi"))
                    return Json(new { success = false, message = "Số điện thoại đã tồn tại." });

                // Xử lý các ngoại lệ một cách thích hợp
                return Json(new { success = false, message = "Lỗi xử lý yêu cầu.", error = ex.Message });
            }


        }

        [HttpPost]
        public ActionResult ThemSachThanhLy(int masach, int soluong)
        {
            try
            {
                KhoSachThanhLy dv = new KhoSachThanhLy();
                dv.masachkho = masach;
                dv.soluongkhotl = soluong;


                var success = _khoThanhLyService.Insert(dv);
                if (success)
                {
                    return Json(new { success = true, message = "Thêm sach thành công." });
                }
                return Json(new { success = false, message = "Thêm sách thất bại." });
            }
            catch (Exception ex)
            {
                // Xử lý các ngoại lệ một cách thích hợp
                return Json(new { success = false, message = "Lỗi xử lý yêu cầu.", error = ex.Message });
            }
        }

        [HttpGet]
        public JsonResult GetSachNhapKho(string keyword)
        {
            var SelectSach = _phieuThanhLyService.GetSachThanhLy(keyword);
            return Json(new ApiOkResponse(SelectSach.ToList()), JsonRequestBehavior.AllowGet);
        }

    }
}