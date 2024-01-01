using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebQuanLyThuVien.Areas.Admin.Data;
using WebQuanLyThuVien.Areas.Admin.Interfaces;
using WebQuanLyThuVien.Interfaces;
using WebQuanLyThuVien.Models;
using WebQuanLyThuVien.Repository;

namespace WebQuanLyThuVien.Areas.Admin.Repository
{
    public class DocGiaRepository : GenericRepository<DocGia>, IDocGiaRepository
    {
        private GenericRepository<DocGia> _repository = null;

        public DocGiaRepository(IUnitOfWork<QuanLyThuVienEntities> unitOfWork) : base(unitOfWork)
        {
            _repository = new GenericRepository<DocGia>(unitOfWork);
        }

        public DocGiaRepository(QuanLyThuVienEntities context) : base(context)
        {
        }

        public IEnumerable<DocGia> GetAll()
        {
            return _repository.Table.ToList();
        }

        public IEnumerable<DTO_DocGia_TheDocGia> GetAllDocGia_TheDocGia()
        {
            return (IEnumerable<DTO_DocGia_TheDocGia>)_repository.Table.ToList();
        }

        public IEnumerable<DTO_DocGia_TheDocGia> GetAllDocGia_PhieuTra()
        {
            return (IEnumerable<DTO_DocGia_TheDocGia>)_repository.Table.ToList();
        }

        public int Delete(int obj)
        {
            throw new NotImplementedException();
        }

        public DocGia GetById(int id)
        {
            throw new NotImplementedException();
        }

        public DocGia GetBySDT(string sdt)
        {
            throw new NotImplementedException();
        }

        public int Insert(DocGia obj)
        {
            throw new NotImplementedException();
        }

        public int Update(DocGia obj)
        {
            throw new NotImplementedException();
        }
    }
}