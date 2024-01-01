using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebQuanLyThuVien.Areas.Admin.Data;
using WebQuanLyThuVien.Models;

namespace WebQuanLyThuVien.Areas.Admin.Interfaces.Services
{
    public interface IPhieuThanhLyService
    {
        IEnumerable<DTO_DonViTL> GetAllDonViTL();
        IEnumerable<DTO_DonViTL> SearchDonVi( string k);
        IEnumerable<KhoThanhLyDTO> GetAllSachTL();

        IEnumerable<KhoThanhLyDTO> SearchSach(string k);
    }
}