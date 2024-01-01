using System.Collections.Generic;
using WebQuanLyThuVien.Areas.Admin.Data;
using WebQuanLyThuVien.Models;

namespace WebQuanLyThuVien.Interfaces.Services
{
    /// <summary>
    /// Khai báo các chức năng cơ bản của 1 entity
    /// 
    /// Nếu có chức năng đặc thù thì khai báo thêm
    /// </summary>
    public interface INhanVienService
    {
        IEnumerable<NhanVien> GetAll();
        DTO_NhanVien_LoginNV GetById(int id);
        void Insert(NhanVien obj);
        void Update(NhanVien obj);
        string Delete(int obj);

        // ví dụ 1 phương thức đặc thù
        NhanVien GetBySDT(string sdt);

        DTO_NhanVien_LoginNV Login(string username, string password);

        void ThemNhanVien(DTO_NhanVien_LoginNV obj);

        void UpdateThongTinNhanVien(DTO_NhanVien_LoginNV obj);
    }
}
