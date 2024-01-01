using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebQuanLyThuVien.Areas.Admin.Data;
using WebQuanLyThuVien.Areas.Admin.Interfaces;
using WebQuanLyThuVien.Areas.Admin.Interfaces.Services;
using WebQuanLyThuVien.Areas.Admin.Repository;
using WebQuanLyThuVien.Interfaces;
using WebQuanLyThuVien.Models;
using WebQuanLyThuVien.Repository;

namespace WebQuanLyThuVien.Areas.Admin.Services
{
    public class PhieuMuonCTPhieuMuonService : IPhieuMuonCTPhieuMuonService
    {
        private UnitOfWork<QuanLyThuVienEntities> unitOfWork = new UnitOfWork<QuanLyThuVienEntities>();

        public void Insert(DTO_Tao_Phieu_Muon x)
        {
            var newPhieuMuon = new PhieuMuon
            {
                NgayMuon = x.NgayMuon,
                HanTra = x.NgayTra,
                MaThe = x.MaTheDocGia,
                MaNV = x.MaNhanVien,
                MaDK = x.MaDK,
            };

            unitOfWork.Context.PhieuMuons.Add(newPhieuMuon);
            unitOfWork.Save(); // Lưu thay đổi vào cơ sở dữ liệu

            foreach (var sachMuon in x.listSachMuon)
            {
                var newChiTietPM = new ChiTietPM
                {
                    MaPM = newPhieuMuon.MaPM,
                    MaSach = sachMuon.MaSach,
                    Soluongmuon = sachMuon.SoLuong
                };

                unitOfWork.Context.ChiTietPMs.Add(newChiTietPM);
            }

            unitOfWork.Save(); // Lưu thay đổi vào cơ sở dữ liệu
        }

        public List<DTO_Sach_Muon> Get_CTPM_ByID(int idPM)
        {
            // Lấy danh sách ChiTietPM có cùng MaPM
            var chiTietPMs = unitOfWork.Context.ChiTietPMs.Where(ctpm => ctpm.MaPM == idPM).ToList();

            // Tạo danh sách để lưu kết quả
            List<DTO_Sach_Muon> listDTO_Sach_Muon = new List<DTO_Sach_Muon>();

            foreach (var ctpm in chiTietPMs)
            {
                // Lấy tên sách tương ứng
                var tenSach = unitOfWork.Context.Saches.Find(ctpm.MaSach)?.TenSach;

                // Tạo đối tượng DTO_Sach_Muon và thêm vào danh sách
                var new_DTO_Sach_Muon = new DTO_Sach_Muon
                {
                    MaSach = ctpm.MaSach,
                    SoLuong = (int)ctpm.Soluongmuon,
                    TenSach = tenSach,
                };

                listDTO_Sach_Muon.Add(new_DTO_Sach_Muon);
            }

            return listDTO_Sach_Muon;
        }

    }
}