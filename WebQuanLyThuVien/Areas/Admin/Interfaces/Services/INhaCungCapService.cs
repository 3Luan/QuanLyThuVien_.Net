using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebQuanLyThuVien.Areas.Admin.Data;
using WebQuanLyThuVien.Models;

namespace WebQuanLyThuVien.Areas.Admin.Interfaces.Services
{
    public interface INhaCungCapService
    {
        IEnumerable<NhaCungCap> GetAll(); //test
        NhaCungCap GetById(int id);

        bool Insert(NhaCungCap obj);
        void Update(NhaCungCap obj);
        string Delete(int obj);
    }
}