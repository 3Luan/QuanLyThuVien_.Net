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
    public class DonViTLService : IDonViTLService
    {
        //private IDocGiaRepository _docGiaRepository;

        private UnitOfWork<QuanLyThuVienEntities> unitOfWork = new UnitOfWork<QuanLyThuVienEntities>();

        public DonViTLService()
        {
        }

        public string Delete(int obj)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<DonViTL> GetAll()
        {
            return unitOfWork.Context.DonViTLs.ToList();
        }

        public DonViTL GetById(int id)
        {
            throw new NotImplementedException();
        }

        public bool Insert(DonViTL obj)
        {

            if (obj.TenDV == "" || obj.SDTDV == "" || obj.DiaChiDV == "")
            {
                return false;
            }
            try

            {
                var existingDonVi = unitOfWork.Context.DonViTLs.FirstOrDefault(dv => dv.SDTDV == obj.SDTDV);

                if (existingDonVi != null)
                {
                    throw new Exception("existingDonVi");
                }
                else
                {
                    var newDonVi = new DonViTL();
                    {

                        newDonVi.MaDV = obj.MaDV;
                        newDonVi.TenDV = obj.TenDV;
                        newDonVi.DiaChiDV = obj.DiaChiDV;
                        newDonVi.SDTDV = obj.SDTDV;
                    };

                    unitOfWork.Context.DonViTLs.Add(newDonVi);

                    unitOfWork.Save();

                    return true;
                }
                
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return false;
            }
            finally
            {
                unitOfWork.Dispose(); // Giải phóng tài nguyên
            }

        }

        public void Update(DonViTL obj)
        {
            throw new NotImplementedException();
        }
    }
}