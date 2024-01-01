using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using WebQuanLyThuVien.Areas.Admin.Data;
using WebQuanLyThuVien.Areas.Admin.Interfaces;
using WebQuanLyThuVien.Areas.Admin.Interfaces.Services;
using WebQuanLyThuVien.Areas.Admin.Repository;
using WebQuanLyThuVien.Models;
using WebQuanLyThuVien.Repository;

namespace WebQuanLyThuVien.Areas.Admin.Services
{
    public class PhieuMuonService : IPhieuMuonService
    {
        private IDocGiaRepository _docGiaRepository;

        private UnitOfWork<QuanLyThuVienEntities> unitOfWork = new UnitOfWork<QuanLyThuVienEntities>();

        public PhieuMuonService()
        {


        }

        public string Delete(int obj)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<PhieuMuon> GetAll()
        {
            throw new NotImplementedException();
        }

        public PhieuMuon GetById(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<PhieuMuon_DTO> GetPhieuMuonsDocGia()
        {
            var listPhieuMuon_DocGia =
                (from PhieuMuon in unitOfWork.Context.PhieuMuons
                 join DocGia in unitOfWork.Context.DocGias
                    on PhieuMuon.MaThe equals DocGia.MaDG
                 join NhanVien in unitOfWork.Context.NhanViens
                 on PhieuMuon.MaNV equals NhanVien.MaNV
                 where PhieuMuon.Tinhtrang == false
                 select new PhieuMuon_DTO
                 {
                     MaPM = PhieuMuon.MaPM,
                     NgayMuon = PhieuMuon.NgayMuon,
                     HanTra = PhieuMuon.HanTra,
                     MaNV = NhanVien.MaNV,
                     MaThe = DocGia.MaDG,
                     HoTenDG = DocGia.HoTenDG,
                     SDT = DocGia.SDT
                 }
                 ).ToList();
            return listPhieuMuon_DocGia;
        }

        public PagingResult<PhieuMuon_DTO> GetAllPhieuMuonPaging(GetListPhieuMuonPaging req)
        {
            var query =
                (from PhieuMuon in unitOfWork.Context.PhieuMuons
                 join DocGia in unitOfWork.Context.DocGias
                    on PhieuMuon.MaThe equals DocGia.MaDG

                 join CHITIETPM in unitOfWork.Context.ChiTietPMs
                 on PhieuMuon.MaPM equals CHITIETPM.MaPM
                 where PhieuMuon.Tinhtrang == false
                  && (string.IsNullOrEmpty(req.Keyword) || DocGia.HoTenDG.Contains(req.Keyword))
                 select new PhieuMuon_DTO
                 {
                     MaPM = PhieuMuon.MaPM,
                     MaThe = DocGia.MaDG,
                     HoTenDG = DocGia.HoTenDG,
                     SDT = DocGia.SDT,
                     NgayMuon = PhieuMuon.NgayMuon,
                     HanTra = PhieuMuon.HanTra

                 }
                ).Distinct().ToList();

            var totalRow = query.Count();

            var listPhieumuons = query.OrderByDescending(x => x.MaPM).Skip((req.Page - 1) * req.PageSize).Take(req.PageSize).ToList();

            return new PagingResult<PhieuMuon_DTO>()
            {
                Results = listPhieumuons,
                CurrentPage = req.Page,
                RowCount = totalRow,
                PageSize = req.PageSize
            };
        }

        public IEnumerable<PhieuMuon_DTO> GetPhieuMuonsChuaTraSach()
        {
            var distinctPhieuMuonNotInPhieuTra =
                (from PhieuMuon in unitOfWork.Context.PhieuMuons
                 join DocGia in unitOfWork.Context.DocGias
                    on PhieuMuon.MaThe equals DocGia.MaDG

                 join CHITIETPM in unitOfWork.Context.ChiTietPMs
                 on PhieuMuon.MaPM equals CHITIETPM.MaPM
                 where PhieuMuon.Tinhtrang == false

                 select new PhieuMuon_DTO
                 {
                     MaPM = PhieuMuon.MaPM,
                     MaThe = DocGia.MaDG,
                     HoTenDG = DocGia.HoTenDG,
                     SDT = DocGia.SDT,
                     NgayMuon = PhieuMuon.NgayMuon,
                     HanTra = PhieuMuon.HanTra

                 }
                ).Distinct().ToList();

            return distinctPhieuMuonNotInPhieuTra;
        }

        public IEnumerable<PhieuMuon_DTO> SearchPhieuMuon(string searchTerm)
        {
            var distinctPhieuMuonNotInPhieuTra =
              (from PhieuMuon in unitOfWork.Context.PhieuMuons
               join DocGia in unitOfWork.Context.DocGias
                  on PhieuMuon.MaThe equals DocGia.MaDG

               join CHITIETPM in unitOfWork.Context.ChiTietPMs
               on PhieuMuon.MaPM equals CHITIETPM.MaPM
               where PhieuMuon.Tinhtrang == false
                       && (DocGia.HoTenDG.Contains(searchTerm)
            || DocGia.SDT.Contains(searchTerm))
               select new PhieuMuon_DTO
               {
                   MaPM = PhieuMuon.MaPM,
                   MaThe = DocGia.MaDG,
                   HoTenDG = DocGia.HoTenDG,
                   SDT = DocGia.SDT,
                   NgayMuon = PhieuMuon.NgayMuon,
                   HanTra = PhieuMuon.HanTra
               }
                ).Distinct().ToList();

            return distinctPhieuMuonNotInPhieuTra;
        }

        public IEnumerable<SachMuonDTO> getSachMuon(int MaPm)
        {
            // Lấy danh sách sách đã trả
            var listSachTra = (
                from phieuTra in unitOfWork.Context.PhieuTras
                join chiTietPT in unitOfWork.Context.ChiTietPTs on phieuTra.MaPT equals chiTietPT.MaPT
                where phieuTra.MaPM == MaPm
                group chiTietPT by chiTietPT.MaSach into g
                select new SachDaTraDTO
                {
                    MaSach = g.Key,
                    SoLuongDaTra = g.Sum(a => a.Soluongtra + a.Soluongloi + a.Soluongmat)
                }
            ).ToList();

            // Lấy danh sách sách mượn
            var sachMuonList = (
                from chiTietPM in unitOfWork.Context.ChiTietPMs
                join sach in unitOfWork.Context.Saches on chiTietPM.MaSach equals sach.MaSach
                join CHITIETPN in unitOfWork.Context.CHITIETPNs on chiTietPM.MaSach equals CHITIETPN.MaSACH
                where chiTietPM.MaPM == MaPm
                select new SachMuonDTO
                {
                    MaSach = sach.MaSach,
                    TenSach = sach.TenSach,
                    SoLuongMuon = chiTietPM.Soluongmuon,
                    giasach = CHITIETPN.GiaSach.Value
                })
                .GroupBy(group => new { group.MaSach, group.TenSach, group.SoLuongMuon })
                .AsEnumerable()
                .Select(x =>
                {
                    // Tìm kiếm thông tin sách đã trả tương ứng
                    var sachDaTra = listSachTra.FirstOrDefault(s => s.MaSach == x.Key.MaSach);

                    // Nếu không tìm thấy, sử dụng giá trị mặc định là 0
                    int soLuongDaTra = sachDaTra?.SoLuongDaTra ?? 0;

                    // Tính toán số lượng còn lại của sách mượn
                    int? soLuongMuonConLaiNullable = x.Key.SoLuongMuon - soLuongDaTra;

                    // Chuyển đổi kiểu dữ liệu từ int? sang int
                    int soLuongMuonConLai = soLuongMuonConLaiNullable ?? 0;


                    // Tạo đối tượng SachMuonDTO mới
                    return new SachMuonDTO
                    {
                        MaSach = x.Key.MaSach,
                        TenSach = x.Key.TenSach,
                        SoLuongMuon = soLuongMuonConLai,
                        giasach = x.OrderByDescending(item => item.giasach).First().giasach
                    };
                })
                .Where(x => x.SoLuongMuon > 0)
                .ToList();

            // Trả về danh sách sách mượn còn lại
            return sachMuonList;
        }


        public void Insert(PhieuMuon obj)
        {
            throw new NotImplementedException();
        }

        public void Update(PhieuMuon obj)
        {
            throw new NotImplementedException();
        }


    }
}