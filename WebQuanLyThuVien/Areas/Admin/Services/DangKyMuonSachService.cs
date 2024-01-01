//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;
//using System.Web.Helpers;
//using WebQuanLyThuVien.Areas.Admin.Controllers;
//using WebQuanLyThuVien.Areas.Admin.Data;
//using WebQuanLyThuVien.Areas.Admin.Interfaces;
//using WebQuanLyThuVien.Models;
//using WebQuanLyThuVien.Repository;

//namespace WebQuanLyThuVien.Areas.Admin.Services
//{
//    public class DangKyMuonSachService
//    {
//        private UnitOfWork<QuanLyThuVienEntities> unitOfWork = new UnitOfWork<QuanLyThuVienEntities>();

//        public IEnumerable<DTO_DangKyMuonSach_GroupSDT> GetAllDangKyMuonSach()
//        {
//            var listDangKyMuonSach =
//                (from DkiMuonSach in unitOfWork.Context.DkiMuonSaches
//                 join LOGIN_DG in unitOfWork.Context.LOGIN_DG
//                    on DkiMuonSach.SDT equals LOGIN_DG.SDT

//                 select new DTO_DangKyMuonSach
//                 {
//                     MaDK = DkiMuonSach.MaDK,
//                     SDT = LOGIN_DG.SDT,
//                     HoTen = LOGIN_DG.HoTen,
//                     Email = LOGIN_DG.Email,
//                     //Password = LOGIN_DG.PASSWORD_DG,
//                     NgayDK = DkiMuonSach.NgayDKMuon,
//                     NgayHen = DkiMuonSach.NgayHen,
//                     TinhTrang = DkiMuonSach.Tinhtrang,

//                 }).AsEnumerable()
//                 .GroupBy(g => g.SDT, (key, g) => new DTO_DangKyMuonSach_GroupSDT
//                 {
//                     SDT = key,
//                     List_DTO_DangKyMuonSach = g.Select(item => new DTO_DangKyMuonSach
//                     {
//                         MaDK = item.MaDK,
//                         SDT = item.SDT,
//                         HoTen = item.HoTen,
//                         Email = item.Email,
//                         //Password = item.Password,
//                         NgayDK = item.NgayDK,
//                         NgayHen = item.NgayHen,
//                         TinhTrang = item.TinhTrang,
//                     }).ToList(),
//                     CountRow = g.Count()
//                 }).ToList();

//            return listDangKyMuonSach;
//        }

//        public List<DTO_Sach_Muon> Get_CTDK_ByMaDK(int maDK)
//        {
//            var sach = unitOfWork.Context.ChiTietDks.Where(ctdk => ctdk.MaDK == maDK).ToList();

//            // Tạo danh sách để lưu kết quả
//            List<DTO_Sach_Muon> listSachDK = new List<DTO_Sach_Muon>();

//            foreach (var ctdk in sach)
//            {
//                // Lấy tên sách tương ứng
//                var tenSach = unitOfWork.Context.Saches.Find(ctdk.MaSach)?.TenSach;

//                // Tạo đối tượng DTO_Sach_Muon và thêm vào danh sách
//                var new_listSachDK = new DTO_Sach_Muon
//                {
//                    MaSach = ctdk.MaSach,
//                    SoLuong = (int)ctdk.Soluongmuon,
//                    TenSach = tenSach,
//                };

//                listSachDK.Add(new_listSachDK);
//            }

//            return listSachDK;
//        }

//        public DTO_DangKyMuonSach Get_DangKySachMuon_ByMaDK(int maDK)
//        {
//            var data =
//                (from DkiMuonSach in unitOfWork.Context.DkiMuonSaches
//                 join LOGIN_DG in unitOfWork.Context.LOGIN_DG
//                    on DkiMuonSach.SDT equals LOGIN_DG.SDT
//                 where DkiMuonSach.MaDK == maDK
//                 select new DTO_DangKyMuonSach
//                 {
//                     MaDK = DkiMuonSach.MaDK,
//                     SDT = LOGIN_DG.SDT,
//                     HoTen = LOGIN_DG.HoTen,
//                     Email = LOGIN_DG.Email,
//                     //Password = LOGIN_DG.PASSWORD_DG,
//                     NgayDK = DkiMuonSach.NgayDKMuon,
//                     NgayHen = DkiMuonSach.NgayHen,
//                     TinhTrang = DkiMuonSach.Tinhtrang,

//                 }).FirstOrDefault();

//            return data;
//        }

//        public bool CheckDocGia(string SDT)
//        {
//            var data = unitOfWork.Context.DocGias
//                        .Where(dg => dg.SDT == SDT)
//                        .FirstOrDefault();

//            return data != null;
//        }

//        public bool UpdateTinhTrang(int maDK, int tinhTrang)
//        {
//            var dkiMuonSach = unitOfWork.Context.DkiMuonSaches
//                .FirstOrDefault(dk => dk.MaDK == maDK);

//            if (dkiMuonSach != null)
//            {
//                dkiMuonSach.Tinhtrang = tinhTrang;
//                unitOfWork.Save();
//                return true;
//            }

//            return false;
//        }

//        public void InsertPhieuMuonDanngKy(DTO_Tao_Phieu_Muon x)
//        {
//            var newPhieuMuon = new PhieuMuon
//            {
//                NgayMuon = x.NgayMuon,
//                HanTra = x.NgayTra,
//                MaThe = x.MaTheDocGia,
//                MaNV = x.MaNhanVien,
//                MaDK = x.MaDK
//            };

//            unitOfWork.Context.PhieuMuons.Add(newPhieuMuon);
//            unitOfWork.Save(); // Lưu thay đổi vào cơ sở dữ liệu

//            foreach (var sachMuon in x.listSachMuon)
//            {
//                var newChiTietPM = new ChiTietPM
//                {
//                    MaPM = newPhieuMuon.MaPM,
//                    MaSach = sachMuon.MaSach,
//                    Soluongmuon = sachMuon.SoLuong
//                };

//                unitOfWork.Context.ChiTietPMs.Add(newChiTietPM);
//            }

//            unitOfWork.Save(); // Lưu thay đổi vào cơ sở dữ liệu
//        }

//        public int GetMaTheBySDT(string sdt)
//        {
//            var data =
//                (from DocGia in unitOfWork.Context.DocGias
//                 join TheDocGia in unitOfWork.Context.TheDocGias
//                    on DocGia.MaDG equals TheDocGia.MaDG
//                 where DocGia.SDT == sdt
//                 select new
//                 {
//                     maThe = TheDocGia.MaThe

//                 }).FirstOrDefault();

//            return data.maThe;
//        }
//        public IEnumerable<DTO_DangKyMuonSach_PM> GetAllDangKyMuonSachtheoTT()
//        {
//            var listDangKyMuonSach =
//                (from DkiMuonSach in unitOfWork.Context.DkiMuonSaches
//                 join LOGIN_DG in unitOfWork.Context.LOGIN_DG
//                    on DkiMuonSach.SDT equals LOGIN_DG.SDT
//                 join DocGia in unitOfWork.Context.DocGias
//                 on DkiMuonSach.SDT equals DocGia.SDT
//                 join TheDocGia in unitOfWork.Context.TheDocGias
//                 on DocGia.MaDG equals TheDocGia.MaThe
//                 where DkiMuonSach.Tinhtrang == 1 && TheDocGia.NgayHH >= DateTime.Now

//                 select new DTO_DangKyMuonSach_PM
//                 {
//                     MaThe = DocGia.MaDG,
//                     MaDK = DkiMuonSach.MaDK,
//                     SDT = LOGIN_DG.SDT,
//                     HoTen = LOGIN_DG.HoTen,
//                     // Email = LOGIN_DG.Email,
//                     //Password = LOGIN_DG.PASSWORD_DG,
//                     NgayDK = DkiMuonSach.NgayDKMuon,
//                     NgayHen = DkiMuonSach.NgayHen,
//                     // TinhTrang = DkiMuonSach.Tinhtrang,

//                 }).ToList();


//            return listDangKyMuonSach;
//        }
//        public bool CheckHanTheDocGia(int maThe)
//        {
//            var data = unitOfWork.Context.TheDocGias
//                       .Where(tdg => tdg.MaThe == maThe && tdg.NgayHH >= DateTime.Now).FirstOrDefault();

//            return data != null;
//        }

//    }
//}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using WebQuanLyThuVien.Areas.Admin.Data;
using WebQuanLyThuVien.Areas.Admin.Interfaces;
using WebQuanLyThuVien.Models;
using WebQuanLyThuVien.Repository;

namespace WebQuanLyThuVien.Areas.Admin.Services
{
    public class DangKyMuonSachService
    {
        private UnitOfWork<QuanLyThuVienEntities> unitOfWork = new UnitOfWork<QuanLyThuVienEntities>();

        public IEnumerable<DTO_DangKyMuonSach_GroupSDT> GetAllDangKyMuonSach()
        {
            var listDangKyMuonSach =
                (from DkiMuonSach in unitOfWork.Context.DkiMuonSaches
                 join LOGIN_DG in unitOfWork.Context.LOGIN_DG
                    on DkiMuonSach.SDT equals LOGIN_DG.SDT

                 select new DTO_DangKyMuonSach
                 {
                     MaDK = DkiMuonSach.MaDK,
                     SDT = LOGIN_DG.SDT,
                     HoTen = LOGIN_DG.HoTen,
                     Email = LOGIN_DG.Email,
                     //Password = LOGIN_DG.PASSWORD_DG,
                     NgayDK = DkiMuonSach.NgayDKMuon,
                     NgayHen = DkiMuonSach.NgayHen,
                     TinhTrang = DkiMuonSach.Tinhtrang,

                 }).AsEnumerable()
                 .GroupBy(g => g.SDT, (key, g) => new DTO_DangKyMuonSach_GroupSDT
                 {
                     SDT = key,
                     List_DTO_DangKyMuonSach = g.Select(item => new DTO_DangKyMuonSach
                     {
                         MaDK = item.MaDK,
                         SDT = item.SDT,
                         HoTen = item.HoTen,
                         Email = item.Email,
                         //Password = item.Password,
                         NgayDK = item.NgayDK,
                         NgayHen = item.NgayHen,
                         TinhTrang = item.TinhTrang,
                     }).ToList(),
                     CountRow = g.Count()
                 }).ToList();

            return listDangKyMuonSach;
        }

        public List<DTO_Sach_Muon> Get_CTDK_ByMaDK(int maDK)
        {
            var sach = unitOfWork.Context.ChiTietDks.Where(ctdk => ctdk.MaDK == maDK).ToList();

            // Tạo danh sách để lưu kết quả
            List<DTO_Sach_Muon> listSachDK = new List<DTO_Sach_Muon>();

            foreach (var ctdk in sach)
            {
                // Lấy tên sách tương ứng
                var tenSach = unitOfWork.Context.Saches.Find(ctdk.MaSach)?.TenSach;

                // Tạo đối tượng DTO_Sach_Muon và thêm vào danh sách
                var new_listSachDK = new DTO_Sach_Muon
                {
                    MaSach = ctdk.MaSach,
                    SoLuong = (int)ctdk.Soluongmuon,
                    TenSach = tenSach,
                };

                listSachDK.Add(new_listSachDK);
            }

            return listSachDK;
        }

        public DTO_DangKyMuonSach Get_DangKySachMuon_ByMaDK(int maDK)
        {
            var data =
                (from DkiMuonSach in unitOfWork.Context.DkiMuonSaches
                 join LOGIN_DG in unitOfWork.Context.LOGIN_DG
                    on DkiMuonSach.SDT equals LOGIN_DG.SDT
                 where DkiMuonSach.MaDK == maDK
                 select new DTO_DangKyMuonSach
                 {
                     MaDK = DkiMuonSach.MaDK,
                     SDT = LOGIN_DG.SDT,
                     HoTen = LOGIN_DG.HoTen,
                     Email = LOGIN_DG.Email,
                     //Password = LOGIN_DG.PASSWORD_DG,
                     NgayDK = DkiMuonSach.NgayDKMuon,
                     NgayHen = DkiMuonSach.NgayHen,
                     TinhTrang = DkiMuonSach.Tinhtrang,

                 }).FirstOrDefault();

            return data;
        }

        public bool CheckDocGia(string SDT)
        {
            var data = unitOfWork.Context.DocGias
                        .Where(dg => dg.SDT == SDT)
                        .FirstOrDefault();

            return data != null;
        }

        public bool CheckHanTheDocGia(int maThe)
        {
            var data = unitOfWork.Context.TheDocGias
                       .Where(tdg => tdg.MaThe == maThe && tdg.NgayHH >= DateTime.Now).FirstOrDefault();

            return data != null;
        }

        public bool UpdateTinhTrang(int maDK, int tinhTrang)
        {
            var dkiMuonSach = unitOfWork.Context.DkiMuonSaches
                .FirstOrDefault(dk => dk.MaDK == maDK);

            if (dkiMuonSach != null)
            {
                dkiMuonSach.Tinhtrang = tinhTrang;
                unitOfWork.Save();
                return true;
            }

            return false;
        }

        public void InsertPhieuMuonDanngKy(DTO_Tao_Phieu_Muon x)
        {
            var newPhieuMuon = new PhieuMuon
            {
                NgayMuon = x.NgayMuon,
                HanTra = x.NgayTra,
                MaThe = x.MaTheDocGia,
                MaNV = x.MaNhanVien,
                MaDK = x.MaDK
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

        public int GetMaTheBySDT(string sdt)
        {
            var data =
                (from DocGia in unitOfWork.Context.DocGias
                 join TheDocGia in unitOfWork.Context.TheDocGias
                    on DocGia.MaDG equals TheDocGia.MaDG
                 where DocGia.SDT == sdt
                 select new
                 {
                     maThe = TheDocGia.MaThe

                 }).FirstOrDefault();

            return data.maThe;
        }
        public IEnumerable<DTO_DangKyMuonSach_PM> GetAllDangKyMuonSachtheoTT()
        {
            var listDangKyMuonSach =
                (from DkiMuonSach in unitOfWork.Context.DkiMuonSaches
                 join LOGIN_DG in unitOfWork.Context.LOGIN_DG
                    on DkiMuonSach.SDT equals LOGIN_DG.SDT
                 join DocGia in unitOfWork.Context.DocGias
                    on DkiMuonSach.SDT equals DocGia.SDT
                 join TheDocGia in unitOfWork.Context.TheDocGias
                    on DocGia.MaDG equals TheDocGia.MaDG
                 where DkiMuonSach.Tinhtrang == 1 && TheDocGia.NgayHH >= DateTime.Now
                 select new DTO_DangKyMuonSach_PM
                 {
                     MaThe = DocGia.MaDG,
                     MaDK = DkiMuonSach.MaDK,
                     SDT = LOGIN_DG.SDT,
                     HoTen = LOGIN_DG.HoTen,
                     // Email = LOGIN_DG.Email,
                     //Password = LOGIN_DG.PASSWORD_DG,
                     NgayDK = DkiMuonSach.NgayDKMuon,
                     NgayHen = DkiMuonSach.NgayHen,
                     // TinhTrang = DkiMuonSach.Tinhtrang,

                 }).ToList();


            return listDangKyMuonSach;
        }
    }
}