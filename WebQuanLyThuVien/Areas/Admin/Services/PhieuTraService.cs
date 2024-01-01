using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;
using WebQuanLyThuVien.Areas.Admin.Controllers;
using WebQuanLyThuVien.Areas.Admin.Data;
using WebQuanLyThuVien.Areas.Admin.Interfaces;
using WebQuanLyThuVien.Areas.Admin.Interfaces.Services;
using WebQuanLyThuVien.Interfaces;
using WebQuanLyThuVien.Models;
using WebQuanLyThuVien.Repository;

namespace WebQuanLyThuVien.Areas.Admin.Services
{
    public class PhieuTraService : IPhieuTraService
    {
        private IDocGiaRepository _docGiaRepository;

        private UnitOfWork<QuanLyThuVienEntities> unitOfWork = new UnitOfWork<QuanLyThuVienEntities>();

        public IEnumerable<PhieuTra_GroupMaPM_DTO> GetAllPhieuTra()
        {
            var listPhieutra_All =
                (from PhieuTra in unitOfWork.Context.PhieuTras
                 join DocGia in unitOfWork.Context.DocGias
                    on PhieuTra.MaThe equals DocGia.MaDG
                 join NhanVien in unitOfWork.Context.NhanViens
                 on PhieuTra.MaNV equals NhanVien.MaNV

                 select new PhieuTra_DTO
                 {
                     MaPT = PhieuTra.MaPT,
                     NgayTra = PhieuTra.NgayTra,
                     MaPM = PhieuTra.MaPM.Value,
                     MaThe = DocGia.MaDG,
                     HoTenDG = DocGia.HoTenDG,
                     SDT = DocGia.SDT,
                     MaNV = NhanVien.MaNV
                 }).AsEnumerable()
                 .GroupBy(g => g.MaPM, (key, g) => new PhieuTra_GroupMaPM_DTO
                 {
                     MaPM = key,
                     DataPhieuTras = g.Select(phieutra => new PhieuTra_DTO
                     {
                         MaPT = phieutra.MaPT,
                         NgayTra = phieutra.NgayTra,
                         MaPM = phieutra.MaPM,
                         MaThe = phieutra.MaThe,
                         HoTenDG = phieutra.HoTenDG,
                         SDT = phieutra.SDT,
                         MaNV = phieutra.MaNV
                     }).ToList(),
                     CountRow = g.Count()
                 }).ToList();

            return listPhieutra_All;
        }
        private List<PhieuTra_GroupMaPM_DTO> listPhieutra_All;
        public List<DTO_Sach_Tra> Get_ChiTietPT_ByMaPM(int maPM)
        {

            var listPhieutra_All =
                (from ChiTietPT in unitOfWork.Context.ChiTietPTs
                 join Sach in unitOfWork.Context.Saches
                      on ChiTietPT.MaSach equals Sach.MaSach

                 join PhieuTra in unitOfWork.Context.PhieuTras
                    on ChiTietPT.MaPT equals PhieuTra.MaPT

                 join PhieuMuon in unitOfWork.Context.PhieuMuons
                     on PhieuTra.MaPM equals PhieuMuon.MaPM


                 where PhieuTra.MaPM == maPM
                 select new DTO_Sach_Tra
                 {
                     MaPT = PhieuTra.MaPT,
                     MaSach = Sach.MaSach,
                     TenSach = Sach.TenSach,
                     //   SoLuongMuon = ChiTietPM.Soluongmuon.Value,
                     SoLuongTra = ChiTietPT.Soluongtra.Value,
                     SoLuongLoi = ChiTietPT.Soluongloi.Value,
                     SoLuongMat = ChiTietPT.Soluongmat.Value,
                 }).ToList();


            return listPhieutra_All.GroupBy(x => new { x.MaPT, x.MaSach, x.SoLuongMuon, x.SoLuongTra, x.SoLuongLoi, x.TenSach, }).Select(x => x.First()).ToList();
        }



        public List<DTO_Sach_Tra> Get_CTPT_ByID(int idPT)
        {
            // Lấy danh sách ChiTietPT có cùng MaPT
            var chiTietPTs = unitOfWork.Context.ChiTietPTs.Where(ctpt => ctpt.MaPT == idPT).ToList();

            // Tạo danh sách để lưu kết quả
            List<DTO_Sach_Tra> listDTO_Sach_Tra = new List<DTO_Sach_Tra>();

            foreach (var ctpt in chiTietPTs)
            {
                // Lấy tên sách tương ứng
                var tenSach = unitOfWork.Context.Saches.Find(ctpt.MaSach)?.TenSach;

                // Tạo đối tượng DTO_Sach_Muon và thêm vào danh sách
                var new_DTO_Sach_Tra = new DTO_Sach_Tra
                {
                    MaPT = ctpt.MaPT,
                    MaSach = ctpt.MaSach,
                    SoLuongTra = (int)ctpt.Soluongtra,
                    SoLuongLoi = (int)ctpt.Soluongloi,
                    SoLuongMat = (int)ctpt.Soluongmat,
                    TenSach = tenSach,
                };

                listDTO_Sach_Tra.Add(new_DTO_Sach_Tra);
            }

            return listDTO_Sach_Tra;
        }

        public PagingResult<PhieuTra_DTO> GetAllPhieuTraPaging(GetListPhieuTraPaging req)
        {
            var query =
                (from PhieuTra in unitOfWork.Context.PhieuTras
                 join DocGia in unitOfWork.Context.DocGias
                    on PhieuTra.MaThe equals DocGia.MaDG
                 join NhanVien in unitOfWork.Context.NhanViens
                 on PhieuTra.MaNV equals NhanVien.MaNV
                 where string.IsNullOrEmpty(req.Keyword) || DocGia.HoTenDG.Contains(req.Keyword)
                 select new PhieuTra_DTO
                 {
                     MaPT = PhieuTra.MaPT,
                     NgayTra = PhieuTra.NgayTra,
                     MaPM = PhieuTra.MaPM.Value,
                     MaThe = DocGia.MaDG,
                     HoTenDG = DocGia.HoTenDG,
                     SDT = DocGia.SDT,
                     MaNV = NhanVien.MaNV
                 });

            //.AsQueryable()
            //.GroupBy(g => g.MaPM, (key, g) => new PhieuTra_GroupMaPM_DTO
            //{
            //    MaPM = key,
            //    DataPhieuTras = g.Select(phieutra => new PhieuTra_DTO
            //    {
            //        MaPT = phieutra.MaPT,
            //        NgayTra = phieutra.NgayTra,
            //        MaPM = phieutra.MaPM,
            //        MaThe = phieutra.MaThe,
            //        HoTenDG = phieutra.HoTenDG,
            //        SDT = phieutra.SDT,
            //        MaNV = phieutra.MaNV
            //    }).ToList(),
            //    CountRow = g.Count()
            //});

            var totalRow = query.Count();

            var listPhieutras = query.OrderByDescending(x => x.MaPM).Skip((req.Page - 1) * req.PageSize).Take(req.PageSize).ToList();

            return new PagingResult<PhieuTra_DTO>()
            {
                Results = listPhieutras,
                CurrentPage = req.Page,
                RowCount = totalRow,
                PageSize = req.PageSize
            };
        }
    }
}