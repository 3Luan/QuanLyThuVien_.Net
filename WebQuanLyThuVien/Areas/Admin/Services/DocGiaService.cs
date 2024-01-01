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
    public class DocGiaService : IDocGiaService
    {

        private IDocGiaRepository _docGiaRepository;

        private UnitOfWork<QuanLyThuVienEntities> unitOfWork = new UnitOfWork<QuanLyThuVienEntities>();

        public DocGiaService()
        {
            _docGiaRepository = new DocGiaRepository(unitOfWork);
        }

        public string Delete(int obj)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<DocGia> GetAll()
        {
            return _docGiaRepository.GetAll().ToList();

        }

        public IEnumerable<DTO_DocGia_TheDocGia> GetAllDocGia_TheDocGia()
        {
            var listDocGia_TheDocGia =
                (from DocGia in unitOfWork.Context.DocGias
                 join TheDocGia in unitOfWork.Context.TheDocGias
                    on DocGia.MaDG equals TheDocGia.MaDG
                 where TheDocGia.NgayHH >= DateTime.Now
                 select new DTO_DocGia_TheDocGia
                 {
                     MaThe = TheDocGia.MaThe,
                     HoTenDG = DocGia.HoTenDG,
                     SDT = DocGia.SDT,
                     DiaChi = DocGia.DiaChi,
                 }
                 ).ToList();
            return listDocGia_TheDocGia;
        }

        public IEnumerable<DTO_DocGia_TheDocGia> GetAllTheDocGia()
        {
            var listTheDocGia =
                (from DocGia in unitOfWork.Context.DocGias
                 join TheDocGia in unitOfWork.Context.TheDocGias
                    on DocGia.MaDG equals TheDocGia.MaDG
                 select new DTO_DocGia_TheDocGia
                 {
                     MaThe = TheDocGia.MaThe,
                     HoTenDG = DocGia.HoTenDG,
                     GioiTinh = DocGia.GioiTinh,
                     NgaySinh = (DateTime)DocGia.NgaySinh,
                     SDT = DocGia.SDT,
                     DiaChi = DocGia.DiaChi,
                     NgayDangKy = (DateTime)TheDocGia.NgayDK,
                     NgayHetHan = (DateTime)TheDocGia.NgayHH,
                     TienThe = (decimal)TheDocGia.TienThe,
                 }
                 ).ToList();
            return listTheDocGia;
        }


        public IEnumerable<DTO_DocGia_TheDocGia> GetAllDocGia_PhieuTra()
        {
            var listDocGia_TheDocGia =
                (from DocGia in unitOfWork.Context.DocGias
                 join TheDocGia in unitOfWork.Context.TheDocGias
                    on DocGia.MaDG equals TheDocGia.MaDG
                 //  where TheDocGia.NgayHH >= DateTime.Now
                 select new DTO_DocGia_TheDocGia
                 {
                     MaThe = TheDocGia.MaThe,
                     HoTenDG = DocGia.HoTenDG,
                     SDT = DocGia.SDT,
                     DiaChi = DocGia.DiaChi,
                 }
                 ).ToList();
            return listDocGia_TheDocGia;
        }

        public DTO_DocGia_TheDocGia GetById(int id)
        {
            try
            {
                var DTO_DocGia_TheDocGia = (
                    from DocGia in unitOfWork.Context.DocGias
                    join TheDocGia in unitOfWork.Context.TheDocGias
                    on DocGia.MaDG equals TheDocGia.MaDG
                    where TheDocGia.MaThe == id
                    select new DTO_DocGia_TheDocGia
                    {
                        MaThe = TheDocGia.MaThe,
                        MaDocGia = DocGia.MaDG,
                        MaNhanVien = (int)TheDocGia.MaNV,
                        HoTenDG = DocGia.HoTenDG,
                        SDT = DocGia.SDT,
                        DiaChi = DocGia.DiaChi,
                        GioiTinh = DocGia.GioiTinh,
                        NgaySinh = (DateTime)DocGia.NgaySinh,
                        NgayDangKy = (DateTime)TheDocGia.NgayDK,
                        NgayHetHan = (DateTime)TheDocGia.NgayHH,
                        TienThe = (int)TheDocGia.TienThe,
                    }).FirstOrDefault();

                return DTO_DocGia_TheDocGia;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetById: {ex.Message}");
                throw;
            }
        }


        public DocGia GetBySDT(string sdt)
        {
            throw new NotImplementedException();
        }

        public void Insert(DocGia obj)
        {
            throw new NotImplementedException();
        }

        public void Update(TheDocGia obj)
        {
            try
            {
                var theDocGiaToUpdate = unitOfWork.Context.TheDocGias.FirstOrDefault(t => t.MaThe == obj.MaThe);

                theDocGiaToUpdate.NgayHH = obj.NgayHH;
                theDocGiaToUpdate.TienThe = obj.TienThe;

                // Lưu thay đổi vào cơ sở dữ liệu
                unitOfWork.Context.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in Update: {ex.Message}");
                throw;
            }
        }

        public void UpdateThongTinDocGia(DocGia obj)
        {
            try
            {
                var existingDocGia = unitOfWork.Context.DocGias.FirstOrDefault(dg => dg.SDT == obj.SDT && dg.MaDG != obj.MaDG);

                if (existingDocGia != null)
                {
                    throw new Exception("Số điện thoại đã tồn tại.");
                }
                else
                {
                    var docGiaUpdate = unitOfWork.Context.DocGias.FirstOrDefault(t => t.MaDG == obj.MaDG);

                    docGiaUpdate.HoTenDG = obj.HoTenDG;
                    docGiaUpdate.NgaySinh = obj.NgaySinh;
                    docGiaUpdate.GioiTinh = obj.GioiTinh;
                    docGiaUpdate.SDT = obj.SDT;
                    docGiaUpdate.DiaChi = obj.DiaChi;

                    // Lưu thay đổi vào cơ sở dữ liệu
                    unitOfWork.Context.SaveChanges();
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in Update: {ex.Message}");
                throw;
            }
        }

        public void DangKyTheDocGia(DTO_DocGia_TheDocGia obj)
        {
            try
            {
                var existingDocGia = unitOfWork.Context.DocGias.FirstOrDefault(dg => dg.SDT == obj.SDT);

                if (existingDocGia != null)
                {
                    throw new Exception("Số điện thoại đã tồn tại.");
                }
                else
                {
                    // Tạo một đối tượng DocGia mới
                    var newDocGia = new DocGia
                    {
                        HoTenDG = obj.HoTenDG,
                        GioiTinh = obj.GioiTinh,
                        NgaySinh = obj.NgaySinh,
                        SDT = obj.SDT,
                        DiaChi = obj.DiaChi,
                    };

                    // Thêm đối tượng DocGia mới vào DbSet DocGias
                    unitOfWork.Context.DocGias.Add(newDocGia);
                    unitOfWork.Save(); // Lưu các thay đổi vào cơ sở dữ liệu

                    // Tạo một đối tượng TheDocGia mới
                    var newTheDocGia = new TheDocGia
                    {
                        MaDG = newDocGia.MaDG, // Sử dụng MaDG từ đối tượng DocGia vừa thêm
                        NgayDK = obj.NgayDangKy,
                        NgayHH = obj.NgayHetHan,
                        TienThe = (int)obj.TienThe,
                        MaNV = obj.MaNhanVien,
                    };

                    // Thêm đối tượng TheDocGia mới vào DbSet TheDocGias
                    unitOfWork.Context.TheDocGias.Add(newTheDocGia);
                    unitOfWork.Save(); // Lưu các thay đổi vào cơ sở dữ liệu
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi trong DangKyTheDocGia: {ex.Message}");
                throw;
            }
        }


    }
}