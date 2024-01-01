using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebQuanLyThuVien.Interfaces;
using WebQuanLyThuVien.Models;

namespace WebQuanLyThuVien.Repository
{
    public class SachRepository : GenericRepository<Sach>, ISachRepository
    {
        private GenericRepository<Sach> _repository = null;

        public SachRepository(IUnitOfWork<QuanLyThuVienEntities> unitOfWork) : base(unitOfWork)
        {

            _repository = new GenericRepository<Sach>(unitOfWork);

        }

        public SachRepository(QuanLyThuVienEntities context) : base(context)
        {
        }

        public IEnumerable<Sach> GetAll()
        {
            return _repository.Table.ToList();
        }

        public int Delete(int obj)
        {
            throw new NotImplementedException();
        }

        public Sach GetById(int id)
        {
            return _repository.GetById(id);
        }

        public int Insert(Sach obj)
        {
            throw new NotImplementedException();
        }

        public int Update(Sach obj)
        {
            throw new NotImplementedException();

        }
    }
}