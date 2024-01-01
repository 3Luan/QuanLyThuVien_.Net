using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using WebQuanLyThuVien.Models;

namespace WebQuanLyThuVien.Areas.Admin.Data
{
    public class SachDTOcs
    {
        public int MaSach { get; set; }
        public string TenSach { get; set; }
        public string TacGia { get; set; }
        public string TheLoai { get; set; }

        public string NgonNgu { get; set; }
        public string NXB { get; set; }
        public int? NamXB { get; set; }
        //public int? GiaSach { get; set; }
        public int? SoLuongHIENTAI { get; set; }
    }


   

}