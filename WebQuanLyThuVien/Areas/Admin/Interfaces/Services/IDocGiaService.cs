using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebQuanLyThuVien.Areas.Admin.Data;
using WebQuanLyThuVien.Models;

namespace WebQuanLyThuVien.Areas.Admin.Interfaces.Services
{
    internal interface IDocGiaService
    {

        IEnumerable<DocGia> GetAll();
        DTO_DocGia_TheDocGia GetById(int id);
        void Insert(DocGia obj);
        void Update(TheDocGia obj);
        string Delete(int obj);

        // ví dụ 1 phương thức đặc thù
        void UpdateThongTinDocGia(DocGia obj);


        DocGia GetBySDT(string sdt);

        IEnumerable<DTO_DocGia_TheDocGia> GetAllDocGia_TheDocGia();

        IEnumerable<DTO_DocGia_TheDocGia> GetAllTheDocGia();

        void DangKyTheDocGia(DTO_DocGia_TheDocGia obj);

    }

}