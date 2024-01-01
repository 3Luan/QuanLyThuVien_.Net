using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using WebQuanLyThuVien.Areas.Admin.Data;
using WebQuanLyThuVien.Interfaces;
using WebQuanLyThuVien.Models;

namespace WebQuanLyThuVien.Repository
{
    /// <summary>
    /// Repository dùng để xử lý dữ liệu với database
    /// </summary>
    public class NhanVienRepository : GenericRepository<NhanVien>, INhanVienRepository
    {

        private GenericRepository<NhanVien> _repository = null;

        public NhanVienRepository(IUnitOfWork<QuanLyThuVienEntities> unitOfWork)
            : base(unitOfWork)
        {
            _repository = new GenericRepository<NhanVien>(unitOfWork);
        }

        public NhanVienRepository(QuanLyThuVienEntities context)
            : base(context)
        {
        }

        public IEnumerable<NhanVien> GetAll()
        {
            return _repository.Table.ToList();
        }

        public NhanVien GetById(int EmployeeID)
        {
            return _repository.GetById(EmployeeID);

        }

        public DTO_NhanVien_LoginNV Login(string username, string password)
        {
            return (DTO_NhanVien_LoginNV)_repository.Login(username, password);
        }

        public int Insert(NhanVien employee)
        {
            //employee.CreateDate = DateTime.Now;      
            return _repository.Insert(employee);
        }

        public int Update(NhanVien employee)
        {
            //employee.ModifiedDate = DateTime.Now;
            //_context.Entry(employee).State = EntityState.Modified;
            return _repository.Update(employee);
        }

        public int Delete(int EmployeeID)
        {
            NhanVien employee = _repository.GetById(EmployeeID);
            return _repository.Delete(employee);
        }

        public NhanVien GetBySDT(string sdt)
        {
            return _repository.Table.FirstOrDefault(nv=> nv.SDT == sdt);
        }
    }
}
