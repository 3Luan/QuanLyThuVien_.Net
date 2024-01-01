using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebQuanLyThuVien.Areas.Admin.Data;
using WebQuanLyThuVien.Areas.Admin.Interfaces.Services;
using WebQuanLyThuVien.Interfaces;
using WebQuanLyThuVien.Models;
using WebQuanLyThuVien.Repository;
using static WebQuanLyThuVien.Areas.Admin.Services.PhieuTraCTPhieuTraService;

namespace WebQuanLyThuVien.Areas.Admin.Services
{
    public class PhieuTraCTPhieuTraService : IPhieuTraCTPhieuTraService
    {

        private UnitOfWork<QuanLyThuVienEntities> unitOfWork = new UnitOfWork<QuanLyThuVienEntities>();


        public IEnumerable<ChiTietPT> GetAllChiTietPT()
        {
            return unitOfWork.Context.ChiTietPTs.ToList();
        }
        public bool Insert(DTO_Tao_Phieu_Tra x)
        {
            if (x.ListSachTra.Any(sach => sach.SoLuongLoi > 0 || sach.SoLuongTra > 0 || sach.SoLuongMat > 0) == false)
            {
                return false;
            }
            try
            {
                var phieuMuon = unitOfWork.Context.PhieuMuons.FirstOrDefault(p => p.MaPM == x.MaPhieuMuon);

                if (phieuMuon == null)
                {
                    return false;
                }


                unitOfWork.CreateTransaction(); // Bắt đầu giao dịch

                // Tạo đối tượng PhieuTra từ DTO_Tao_Phieu_Tra
                var newPhieuTra = new PhieuTra
                {
                    NgayTra = x.NgayTra,
                    MaThe = phieuMuon.MaThe,
                    MaNV = x.MaNhanVien,
                    MaPM = x.MaPhieuMuon,
                };

                // Thêm PhieuTra vào Context
                unitOfWork.Context.PhieuTras.Add(newPhieuTra);

                // Duyệt qua danh sách sách trả và tạo đối tượng ChiTietPT cho mỗi cuốn sách
                foreach (var sachtra in x.ListSachTra)
                {
                    if (sachtra.SoLuongTra == 0 && sachtra.SoLuongLoi == 0 && sachtra.SoLuongMat == 0) 
                    {
                        continue;
                    }
                    var newChiTietPT = new ChiTietPT
                    {
                        MaPT = newPhieuTra.MaPT,
                        MaSach = sachtra.MaSach,
                        Soluongtra = sachtra.SoLuongTra,
                        Soluongloi = sachtra.SoLuongLoi,
                        Soluongmat = sachtra.SoLuongMat,
                        PhuThu = sachtra.PhuThu,
                    };

                    // Thêm ChiTietPT vào Context
                    var a = unitOfWork.Context.ChiTietPTs.Add(newChiTietPT);
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
    }


}