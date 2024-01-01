using Microsoft.Ajax.Utilities;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using WebQuanLyThuVien.Areas.Admin.Data;
using WebQuanLyThuVien.Areas.Admin.Services;
using WebQuanLyThuVien.Interfaces.Services;
using WebQuanLyThuVien.Models;
using WebQuanLyThuVien.Services;

namespace WebQuanLyThuVien.Areas.Admin.Controllers
{
    public class PhieuMuonController : Controller
    {
        SachService _sachService = new SachService();
        DocGiaService _docGiaService = new DocGiaService();
        PhieuMuonCTPhieuMuonService _phieuMuonCTPhieuMuonService = new PhieuMuonCTPhieuMuonService();
        DangKyMuonSachService _dangKyMuonSachService = new DangKyMuonSachService();

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
                var sach = _sachService.GetAll();
                var docGia = _docGiaService.GetAllDocGia_TheDocGia();
                var dangky = _dangKyMuonSachService.GetAllDangKyMuonSachtheoTT();
                ViewData["Sach"] = sach;
                ViewData["DocGia"] = docGia;
                ViewData["DkiSachOnl"] = dangky;
                Session["LoaiClick"] = "2";
                return View();
            }
        }

        /// <summary>
        /// loai 1 dnag ky , 2 bin hthuong
        /// </summary>
        /// <param name="MaSach"></param>
        /// <param name="TenSach"></param>
        /// <param name="SoLuong"></param>
        /// <param name="MaDK"></param>
        /// <returns></returns>
        /// 

        [HttpGet]
        public ActionResult GetAllThongTinDangKy()
        {
            var dangky = _dangKyMuonSachService.GetAllDangKyMuonSachtheoTT();
            return Json(new ApiOkResponse(dangky.ToList()), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult ThemSachMuon(int MaSach, string TenSach, int SoLuong, int MaDK)
        {
            List<DTO_Sach_Muon> listSachMuon;
            if (MaDK > 0)
            {

                var data = _dangKyMuonSachService.Get_CTDK_ByMaDK(MaDK);
                if (Session["LoaiClick"].ToString() == "2")
                {
                    Session["ListSachMuon"] = new List<DTO_Sach_Muon>();
                }
                if (data == null)
                {
                    listSachMuon = new List<DTO_Sach_Muon>();
                }
                else
                {
                    listSachMuon = data;
                }

                Session["ListSachMuon"] = listSachMuon;
                Session["LoaiClick"] = "1";
                // Trả về một JsonResult chứa danh sách sách đã cập nhật
                return Json(listSachMuon);

            }
            else
            {
                if (Session["LoaiClick"].ToString() == "1")
                {
                    Session["ListSachMuon"] = new List<DTO_Sach_Muon>();
                }
                // Lấy danh sách sách đã mượn từ Session hoặc tạo danh sách mới nếu chưa tồn tại
                if (Session["ListSachMuon"] == null)
                {
                    listSachMuon = new List<DTO_Sach_Muon>();
                }
                else
                {
                    listSachMuon = (List<DTO_Sach_Muon>)Session["ListSachMuon"];
                }

                // Tìm xem sách có MaSach trong danh sách chưa
                var existingSach = listSachMuon.FirstOrDefault(s => s.MaSach == MaSach);

                if (existingSach != null)
                {
                    // Nếu đã tồn tại, tăng số lượng
                    existingSach.SoLuong += SoLuong;
                }
                else
                {
                    // Nếu chưa tồn tại, thêm sách mới vào danh sách
                    var sachMoi = new DTO_Sach_Muon
                    {
                        MaSach = MaSach,
                        TenSach = TenSach,
                        SoLuong = SoLuong
                    };

                    listSachMuon.Add(sachMoi);
                }
                // Lưu danh sách đã cập nhật vào Session
                Session["ListSachMuon"] = listSachMuon;
                Session["LoaiClick"] = "2";
                // Trả về một JsonResult chứa danh sách sách đã cập nhật
                return Json(listSachMuon);
            }

        }


        [HttpPost]
        public ActionResult LamMoiDanhSachSachMuon()
        {
            Session["ListSachMuon"] = null;
            return Json(new { success = true });
        }


        [HttpPost]
        public ActionResult XoaSachMuon(int MaSach)
        {
            // Lấy danh sách sách đã mượn từ Session hoặc tạo danh sách mới nếu chưa tồn tại
            List<DTO_Sach_Muon> listSachMuon = Session["ListSachMuon"] as List<DTO_Sach_Muon> ?? new List<DTO_Sach_Muon>();

            // Tìm và xóa sách khỏi danh sách dựa trên mã sách
            var sachXoa = listSachMuon.FirstOrDefault(s => s.MaSach == MaSach);
            if (sachXoa != null)
            {
                listSachMuon.Remove(sachXoa);
                Session["ListSachMuon"] = listSachMuon;
                return Json(new { success = true });
            }

            return Json(new { success = false });
        }


        [HttpPost]
        public ActionResult TaoPhieuMuon(int MaNhanVien, int MaThe, DateTime NgayMuon, DateTime NgayTra, int MaDK)
        {
            DTO_Tao_Phieu_Muon tpm = new DTO_Tao_Phieu_Muon();

            tpm.MaNhanVien = MaNhanVien;
            tpm.MaTheDocGia = MaThe;
            tpm.NgayMuon = NgayMuon;
            tpm.NgayTra = NgayTra;
            tpm.MaDK = MaDK;
            tpm.listSachMuon = Session["ListSachMuon"] as List<DTO_Sach_Muon>;

            if (Session["ListSachMuon"] as List<DTO_Sach_Muon> == null)
                return Json(new { success = false });
            else
            {
                _phieuMuonCTPhieuMuonService.Insert(tpm);

                if (tpm.MaDK != 0)
                {
                    _dangKyMuonSachService.UpdateTinhTrang(tpm.MaDK, 2);
                }

                return Json(new { success = true });
            }
        }



    }
}