using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebQuanLyThuVien.Areas.Admin.Data;
using WebQuanLyThuVien.Models;

namespace WebQuanLyThuVien.Areas.Admin.Interfaces.Services
{
    public interface IPhieuMuonService
    {
        IEnumerable<PhieuMuon> GetAll();
        PhieuMuon GetById(int id);
        void Insert(PhieuMuon obj);
        void Update(PhieuMuon obj);
        string Delete(int obj);

        IEnumerable<PhieuMuon_DTO> GetPhieuMuonsDocGia();

        IEnumerable<PhieuMuon_DTO> SearchPhieuMuon(String searchTerm);
        IEnumerable<PhieuMuon_DTO> GetPhieuMuonsChuaTraSach();
        IEnumerable<SachMuonDTO> getSachMuon(int mapm);
    }
}
