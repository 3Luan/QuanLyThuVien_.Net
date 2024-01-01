using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using WebQuanLyThuVien.Models;

namespace WebQuanLyThuVien.Areas.Admin.Data
{
    public class PhieuTra_DTO
    {

        public int MaPT { get; set; }
        public DateTime? NgayTra { get; set; }
      
        public int MaNV { get; set; }

        public String HoTenNV { get; set; }

        public int MaPM { get; set; }

        public int MaThe { get; set; }
        public String HoTenDG { get; set; } 
        public String SDT { get; set; }

    }

    public class PhieuTra_GroupMaPM_DTO
    {
        public int MaPM { get; set; }
        public int CountRow { get; set; }
        public List<PhieuTra_DTO> DataPhieuTras { get; set; }
    }
}