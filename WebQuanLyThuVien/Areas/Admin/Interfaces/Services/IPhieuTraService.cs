using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebQuanLyThuVien.Areas.Admin.Data;

namespace WebQuanLyThuVien.Areas.Admin.Interfaces.Services
{
    public interface IPhieuTraService
    {
        IEnumerable<PhieuTra_GroupMaPM_DTO> GetAllPhieuTra();
        PagingResult<PhieuTra_DTO> GetAllPhieuTraPaging(GetListPhieuTraPaging req);
    }
}