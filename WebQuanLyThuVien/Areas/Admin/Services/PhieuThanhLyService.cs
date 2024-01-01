using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebQuanLyThuVien.Areas.Admin.Data;
using WebQuanLyThuVien.Areas.Admin.Interfaces;
using WebQuanLyThuVien.Areas.Admin.Interfaces.Services;
using WebQuanLyThuVien.Models;
using WebQuanLyThuVien.Repository;

namespace WebQuanLyThuVien.Areas.Admin.Services
{
    public class PhieuThanhLyService : IPhieuThanhLyService
    {

        private UnitOfWork<QuanLyThuVienEntities> unitOfWork = new UnitOfWork<QuanLyThuVienEntities>();

        public IEnumerable<DTO_DonViTL> GetAllDonViTL()
        {
            var listDonViTL_All =
                (from DonViTL in unitOfWork.Context.DonViTLs

                 select new DTO_DonViTL
                 {
                     MaDV = DonViTL.MaDV,
                     TenDV = DonViTL.TenDV,
                     SDTDV = DonViTL.SDTDV,
                     DiaChiDV = DonViTL.DiaChiDV,

                 }).ToList();

            return listDonViTL_All;
        }

        public IEnumerable<DTO_DonViTL> SearchDonVi(string searchTerm)
        {
            var distinctdonvi =
              (from DonViTL in unitOfWork.Context.DonViTLs
               where (DonViTL.TenDV.Contains(searchTerm)
            || DonViTL.SDTDV.Contains(searchTerm))
               select new DTO_DonViTL
               {
                   MaDV = DonViTL.MaDV,
                   TenDV = DonViTL.TenDV,
                   SDTDV = DonViTL.SDTDV,
                   DiaChiDV = DonViTL.DiaChiDV,

               }
                ).ToList();

            return distinctdonvi;
        }


        public IEnumerable<KhoThanhLyDTO> GetAllSachTL()
        {
            var listSachTL_All =
                (from KhoSachThanhLy in unitOfWork.Context.KhoSachThanhLies
                 join Sach in unitOfWork.Context.Saches on KhoSachThanhLy.masachkho equals Sach.MaSach
                 join CHITIETPN in unitOfWork.Context.CHITIETPNs on KhoSachThanhLy.masachkho equals CHITIETPN.MaSACH
                 select new KhoThanhLyDTO
                 {
                     MaSachKho = KhoSachThanhLy.masachkho,
                     TenSach = Sach.TenSach,
                     SoLuongKhoTL = KhoSachThanhLy.soluongkhotl.Value,
                     GiaSachTL = (decimal)(CHITIETPN.GiaSach.Value * 30 / 100),
                 })
                .GroupBy(group => new { group.MaSachKho, group.TenSach, group.SoLuongKhoTL })
                .AsEnumerable()
                .Select(x =>
                {
                    // Tạo đối tượng SachMuonDTO mới
                    return new KhoThanhLyDTO
                    {
                        MaSachKho = x.Key.MaSachKho,
                        TenSach = x.Key.TenSach,
                        SoLuongKhoTL = x.Key.SoLuongKhoTL,
                        GiaSachTL = x.OrderByDescending(item => item.GiaSachTL).First().GiaSachTL
                    };
                })
                .Where(x => x.SoLuongKhoTL > 0)
                .ToList();

            return listSachTL_All;
        }




        public IEnumerable<KhoThanhLyDTO> SearchSach(string searchTerm)
        {
            var distinctSach =
                (from KhoSachThanhLy in unitOfWork.Context.KhoSachThanhLies
                 join Sach in unitOfWork.Context.Saches on KhoSachThanhLy.masachkho equals Sach.MaSach
                 join CHITIETPN in unitOfWork.Context.CHITIETPNs on KhoSachThanhLy.masachkho equals CHITIETPN.MaSACH
                 where Sach.TenSach.Contains(searchTerm)
                 select new KhoThanhLyDTO
                 {
                     MaSachKho = KhoSachThanhLy.masachkho,
                     TenSach = Sach.TenSach,
                     SoLuongKhoTL = KhoSachThanhLy.soluongkhotl.Value,
                     GiaSachTL = (decimal)(CHITIETPN.GiaSach.Value * 30 / 100),
                 })
                .GroupBy(group => new { group.MaSachKho, group.TenSach, group.SoLuongKhoTL })
                .AsEnumerable()
                .Select(x =>
                {
                    // Tạo đối tượng SachMuonDTO mới
                    return new KhoThanhLyDTO
                    {
                        MaSachKho = x.Key.MaSachKho,
                        TenSach = x.Key.TenSach,
                        SoLuongKhoTL = x.Key.SoLuongKhoTL,
                        GiaSachTL = x.OrderByDescending(item => item.GiaSachTL).First().GiaSachTL
                    };
                })
                .Where(x => x.SoLuongKhoTL > 0)
                .ToList();

            //return listSachTL_All;
            //var distinctSach =
            //  (from Sach in unitOfWork.Context.Saches
            //   join KhoSachThanhLy in unitOfWork.Context.KhoSachThanhLies
            //      on Sach.MaSach equals KhoSachThanhLy.masachkho
            //   where Sach.TenSach.Contains(searchTerm)
            //   select new KhoThanhLyDTO
            //   {
            //       MaSachKho = KhoSachThanhLy.masachkho,
            //       TenSach = Sach.TenSach,
            //       SoLuongKhoTL = KhoSachThanhLy.soluongkhotl.Value,
            //   }
            //    ).Distinct().ToList();

            return distinctSach;
        }

        public bool InsertSachThanhLy(DTO_Tao_Phieu_TL data)
        {

            if (data.listSachTL.Any(sach => sach.SoLuong > 0) == false)
            {
                return false;
            }
            try
            {
               
                unitOfWork.CreateTransaction(); // Bắt đầu giao dịch

                // Tạo đối tượng PhieuTra từ DTO_Tao_Phieu_Tra
                var newPhieuThanhLy = new PhieuThanhLy
                {
                    NgayTL = DateTime.Now,
                    MaDV = data.MaDonVi,
                    MaNV = data.MaNhanVien,
                };

                // Thêm PhieuTra vào Context
                unitOfWork.Context.PhieuThanhLies.Add(newPhieuThanhLy);

                // Duyệt qua danh sách sách trả và tạo đối tượng ChiTietPT cho mỗi cuốn sách
                foreach (var sachThanhLy in data.listSachTL)
                {
                    if (sachThanhLy.SoLuong <= 0)
                    {
                        continue;
                    }

                    var chiTietPTL = new ChiTietPTL
                    {
                        GiaTL = sachThanhLy.GiaSach,
                        MaPTL = newPhieuThanhLy.MaPTL,
                        MaSachkho = sachThanhLy.MaSach,
                        Soluongtl = sachThanhLy.SoLuong
                    };

                    // Thêm ChiTietPT vào Context
                    var a = unitOfWork.Context.ChiTietPTLs.Add(chiTietPTL);
                }

                // Lưu thay đổi vào cơ sở dữ liệu khi mọi thứ đã thành công
                unitOfWork.Commit();

                unitOfWork.Save();

                return true;
            }
            catch (Exception ex)
            {
                unitOfWork.Rollback(); // Rollback nếu có lỗi
                Console.WriteLine($"Error: {ex.Message}");
                // Xử lý lỗi và ghi log
                return false;
            }
            finally
            {
                unitOfWork.Dispose(); // Giải phóng tài nguyên
            }
        }
        public IEnumerable<DTO_Sach_Nhap_Kho> GetSachThanhLy(string searchTerm)
        {
            if (string.IsNullOrEmpty(searchTerm))
            {
                return unitOfWork.Context.Saches.Select(s => new DTO_Sach_Nhap_Kho
                {
                    MaSach = s.MaSach,
                    TenSach = s.TenSach,
                    SoLuongHienTai = s.SoLuongHIENTAI.Value,
                }).ToList();
            }

            var distinctSach =
              (from Sach in unitOfWork.Context.Saches
               where Sach.TenSach.Contains(searchTerm)
               select new DTO_Sach_Nhap_Kho
               {
                   MaSach = Sach.MaSach,
                   TenSach = Sach.TenSach,
                   SoLuongHienTai = Sach.SoLuongHIENTAI.Value,
               }
                ).Distinct().ToList();

            return distinctSach;
        }
    }
}