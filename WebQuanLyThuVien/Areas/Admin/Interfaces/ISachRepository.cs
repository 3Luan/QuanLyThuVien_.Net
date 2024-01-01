using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebQuanLyThuVien.Models;

namespace WebQuanLyThuVien.Interfaces
{
    public interface ISachRepository
    {
        IEnumerable<Sach> GetAll();
        Sach GetById(int id);
        int Insert(Sach obj);
        int Update(Sach obj);
        int Delete(int obj);

    }
}

