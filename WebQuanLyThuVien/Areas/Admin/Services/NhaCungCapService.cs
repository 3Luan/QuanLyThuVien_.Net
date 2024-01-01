using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebQuanLyThuVien.Areas.Admin.Data;
using WebQuanLyThuVien.Areas.Admin.Interfaces;
using WebQuanLyThuVien.Areas.Admin.Interfaces.Services;
using WebQuanLyThuVien.Models;
using WebQuanLyThuVien.Repository;

namespace WebQuanLyThuVien.Areas.Admin.Services
{
    public class NhaCungCapService : INhaCungCapService
    {
        //private IDocGiaRepository _docGiaRepository;

        private UnitOfWork<QuanLyThuVienEntities> unitOfWork = new UnitOfWork<QuanLyThuVienEntities>();

        public NhaCungCapService()
        {
        }

        public string Delete(int obj)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<NhaCungCap> GetAll()
        {
            var nhaCungCap =
                (from NhaCungCap in unitOfWork.Context.NhaCungCaps
                 select new
                 {
                     NhaCungCap.MaNCC,
                     NhaCungCap.TenNCC,
                     NhaCungCap.DiaChiNCC,
                     NhaCungCap.sdtNCC,
                 })
                .ToList() // Materialize the query before the subsequent projection
                .Select(x => new NhaCungCap
                {
                    MaNCC = x.MaNCC,
                    TenNCC = x.TenNCC,
                    DiaChiNCC = x.DiaChiNCC,
                    sdtNCC = x.sdtNCC,
                })
                .ToList();

            return nhaCungCap;
        }
        public PagingResult<NhaCungCap> GetAllNCCPaging(GetListPhieuTraPaging req)
        {
            var query =
                 (from NhaCungCap in unitOfWork.Context.NhaCungCaps
                  where string.IsNullOrEmpty(req.Keyword) || NhaCungCap.TenNCC.Contains(req.Keyword)
                  select new
                  {
                      NhaCungCap.MaNCC,
                      NhaCungCap.TenNCC,
                      NhaCungCap.DiaChiNCC,
                      NhaCungCap.sdtNCC,
                  })
                .ToList() // Materialize the query before the subsequent projection
                .Select(x => new NhaCungCap
                {
                    MaNCC = x.MaNCC,
                    TenNCC = x.TenNCC,
                    DiaChiNCC = x.DiaChiNCC,
                    sdtNCC = x.sdtNCC,
                })
                .ToList();

            var totalRow = query.Count();

            var listNCCs = query.OrderByDescending(x => x.MaNCC).Skip((req.Page - 1) * req.PageSize).Take(req.PageSize).ToList();

            return new PagingResult<NhaCungCap>()
            {
                Results = listNCCs,
                CurrentPage = req.Page,
                RowCount = totalRow,
                PageSize = req.PageSize
            };
        }

        public NhaCungCap GetById(int id)
        {
            throw new NotImplementedException();
        }

        public bool Insert(NhaCungCap obj)
        {
            unitOfWork.Context.NhaCungCaps.Add(obj);

            if (obj.TenNCC == "" || obj.sdtNCC == "" || obj.DiaChiNCC == "")
            {
                return false;
            }
            try
            {
                unitOfWork.CreateTransaction(); // Bắt đầu giao dịch

                // Lưu thay đổi vào cơ sở dữ liệu khi mọi thứ đã thành công
                unitOfWork.Commit();

                unitOfWork.Save();

                return true;
            }
            catch (Exception ex)
            {
                unitOfWork.Rollback(); // Rollback nếu có lỗi
                Console.WriteLine($"Error: {ex.Message}");
                // Xử lý lỗi và ghi log
                return false;
            }
            finally
            {
                unitOfWork.Dispose(); // Giải phóng tài nguyên
            }

        }

        public void Update(NhaCungCap obj)
        {
            throw new NotImplementedException();
        }
    }
}