using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebQuanLyThuVien.Areas.Admin.Data;
using WebQuanLyThuVien.Models;

namespace WebQuanLyThuVien.Interfaces.Services
{
    public interface ISachService
    {

        IEnumerable<Sach> GetAll();
        SachDTOcs GetById(int id);
        void Insert(Sach obj);
        void Update(Sach obj);
        string Delete(int obj);

        // ví dụ 1 phương thức đặc thù

        IEnumerable<SachDTOcs> GetSACH();
    }
}