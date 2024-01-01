using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebQuanLyThuVien.Models;

namespace WebQuanLyThuVien.Areas.Admin.Interfaces.Services
{
    public interface IDonViTLService
    {
        IEnumerable<DonViTL> GetAll();
        DonViTL GetById(int id);
        bool Insert(DonViTL obj);
        void Update(DonViTL obj);
        string Delete(int obj);
    }
}