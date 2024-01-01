using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;

namespace WebQuanLyThuVien.Areas.Admin.Data
{
    public class KhoThanhLyDTO
    {
        public int MaSachKho { get; set; }

        public string TenSach { get; set; }
        public int SoLuongKhoTL { get; set; }
        public decimal GiaSachTL { get; set; }
    }

    public class DTO_DonViTL
    {
        public int MaDV { get; set; }
        public string TenDV { get; set; }
        public string DiaChiDV { get; set; }
        public string SDTDV { get; set; }
    }
    public class DTO_Sach_Tl
    {
        public int MaSach { get; set; }
        public string TenSach { get; set; }
        public int SoLuong { get; set; }

        public decimal GiaSach { get; set; }
    }

    public class DTO_Tao_Phieu_TL
    {
        public int MaNhanVien { get; set; }
        public int MaDonVi { get; set; }
        public DateTime NgayTL { get; set; } = DateTime.Now;
    
        public List<DTO_Sach_Tl> listSachTL { get; set; }

        public DTO_Tao_Phieu_TL()
        {
            listSachTL = new List<DTO_Sach_Tl>();
        }
    }

    public class DTO_Sach_Nhap_Kho
    {
        public int MaSach { get; set; }
        public string TenSach { get; set; }
        public int SoLuongHienTai { get; set; }

        public decimal GiaSach { get; set; }
    }

}