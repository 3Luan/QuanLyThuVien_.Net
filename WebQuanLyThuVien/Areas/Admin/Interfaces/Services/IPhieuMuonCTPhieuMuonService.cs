using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebQuanLyThuVien.Areas.Admin.Data;
using WebQuanLyThuVien.Models;

namespace WebQuanLyThuVien.Areas.Admin.Interfaces.Services
{
    public interface IPhieuMuonCTPhieuMuonService
    {
        void Insert(DTO_Tao_Phieu_Muon obj);

        List<DTO_Sach_Muon> Get_CTPM_ByID(int id);
    }
}