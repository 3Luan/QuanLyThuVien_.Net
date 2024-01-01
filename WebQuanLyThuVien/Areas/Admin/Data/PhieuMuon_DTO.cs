using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using WebQuanLyThuVien.Models;

namespace WebQuanLyThuVien.Areas.Admin.Data
{
    public class PhieuMuon_DTO
    {

        public int MaPM { get; set; }
        public DateTime? NgayMuon { get; set; }
        public DateTime? HanTra { get; set; }

        public int MaNV { get; set; }

        public String HoTenNV { get; set; }
        public int MaThe { get; set; }
        public String HoTenDG { get; set; }
        public String SDT { get; set; }
        public bool Tinhtrang { get; set; }

        public int MaDK { get; set; }


    }

}


