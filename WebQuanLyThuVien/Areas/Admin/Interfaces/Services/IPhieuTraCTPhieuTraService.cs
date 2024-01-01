using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebQuanLyThuVien.Areas.Admin.Data;

namespace WebQuanLyThuVien.Areas.Admin.Interfaces.Services
{
    public interface IPhieuTraCTPhieuTraService
    {
        bool Insert(DTO_Tao_Phieu_Tra obj);
    }
}