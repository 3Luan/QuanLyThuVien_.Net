using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebQuanLyThuVien.Areas.Admin.Data;
using WebQuanLyThuVien.Interfaces;
using WebQuanLyThuVien.Interfaces.Services;
using WebQuanLyThuVien.Models;
using WebQuanLyThuVien.Repository;

namespace WebQuanLyThuVien.Services
{


    public class SachService : ISachService
    {

        private ISachRepository _sachRepository;

        private UnitOfWork<QuanLyThuVienEntities> unitOfWork = new UnitOfWork<QuanLyThuVienEntities>();

        public SachService()
        {
            _sachRepository = new SachRepository(unitOfWork);
        }

        public string Delete(int obj)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Sach> GetAll()
        {
            return _sachRepository.GetAll().ToList();
        }

        public SachDTOcs GetById(int id)
        {
            try
            {
                return (from SACH in unitOfWork.Context.Saches
                        where SACH.MaSach == id
                        select new SachDTOcs
                        {
                            MaSach = SACH.MaSach,
                            TenSach = SACH.TenSach,
                            TheLoai = SACH.TheLoai,
                            TacGia = SACH.TacGia,
                            NgonNgu = SACH.NgonNgu,
                            NXB = SACH.NXB,
                            NamXB = SACH.NamXB,
                            SoLuongHIENTAI = SACH.SoLuongHIENTAI
                        }).FirstOrDefault();
            }
            catch (Exception ex)
            {
                // Xử lý lỗi nếu có
                Console.WriteLine($"Error in GetById: {ex.Message}");
                return null;
            }
        }




        public IEnumerable<SachDTOcs> GetSACH()
        {
            var listSACH =
                (from SACH in unitOfWork.Context.Saches

                 select new SachDTOcs
                 {
                     MaSach = SACH.MaSach,
                     TenSach = SACH.TenSach,
                     TacGia = SACH.TacGia,
                     /*  TheLoai = SACH.TheLoai,
                       NgonNgu= SACH.NgonNgu,
                       NXB  = SACH.NXB,
                       NamXB = SACH.NamXB,
                       GiaSach = SACH.GiaSach, */
                     SoLuongHIENTAI = SACH.SoLuongHIENTAI
                 }
                 ).ToList();
            return listSACH;

        }

        public void Insert(Sach obj)
        {
        }

        public void Update(Sach obj)
        {

        }

        public PagingResult<SachDTOcs> GetAllSachPaging(GetListPhieuTraPaging req)
        {
            var query =
                (from SACH in unitOfWork.Context.Saches
                 where string.IsNullOrEmpty(req.Keyword) || SACH.TenSach.Contains(req.Keyword)
                 select new SachDTOcs
                 {
                     MaSach = SACH.MaSach,
                     TenSach = SACH.TenSach,
                     TacGia = SACH.TacGia,
                     /*  TheLoai = SACH.TheLoai,
                       NgonNgu= SACH.NgonNgu,
                       NXB  = SACH.NXB,
                       NamXB = SACH.NamXB,
                       GiaSach = SACH.GiaSach, */
                     SoLuongHIENTAI = SACH.SoLuongHIENTAI
                 }
                 ).ToList();



            var totalRow = query.Count();

            var listSachs = query.OrderByDescending(x => x.MaSach).Skip((req.Page - 1) * req.PageSize).Take(req.PageSize).ToList();

            return new PagingResult<SachDTOcs>()
            {
                Results = listSachs,
                CurrentPage = req.Page,
                RowCount = totalRow,
                PageSize = req.PageSize
            };
        }
    }
}