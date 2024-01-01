using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebQuanLyThuVien.Areas.Admin.Data;
using WebQuanLyThuVien.Models;

namespace WebQuanLyThuVien.Areas.Admin.Interfaces
{
    internal interface IDocGiaRepository
    {
        IEnumerable<DocGia> GetAll();
        DocGia GetById(int id);
        int Insert(DocGia obj);
        int Update(DocGia obj);
        int Delete(int obj);

        DocGia GetBySDT(string sdt);
        IEnumerable<DTO_DocGia_TheDocGia> GetAllDocGia_TheDocGia();

    }
}
