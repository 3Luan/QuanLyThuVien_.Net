using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebQuanLyThuVien.Models;

namespace WebQuanLyThuVien.Areas.Admin.Interfaces.Services
{
    public interface IKhoThanhLyService
    {
        IEnumerable<KhoSachThanhLy> GetAll();
        //DonViTL GetById(int id);
        bool Insert(KhoSachThanhLy x);
        void Update(KhoSachThanhLy obj);
        string Delete(int obj);
    }
}