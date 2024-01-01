using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;
using WebQuanLyThuVien.Areas.Admin.Data;
using WebQuanLyThuVien.Areas.Admin.Interfaces;
using WebQuanLyThuVien.Areas.Admin.Interfaces.Services;
using WebQuanLyThuVien.Models;
using WebQuanLyThuVien.Repository;

namespace WebQuanLyThuVien.Areas.Admin.Services
{
    public class KhoThanhLyService : IKhoThanhLyService
    {
        //private IDocGiaRepository _docGiaRepository;

        private UnitOfWork<QuanLyThuVienEntities> unitOfWork = new UnitOfWork<QuanLyThuVienEntities>();

        public KhoThanhLyService()
        {
        }

        public string Delete(int obj)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<KhoSachThanhLy> GetAll()
        {
            return unitOfWork.Context.KhoSachThanhLies.ToList();
        }

        public KhoThanhLyDTO GetById(int id)
        {
            throw new NotImplementedException();
        }

       
        public bool Insert(KhoSachThanhLy x)
        {
            if (x.masachkho <= 0 || x.soluongkhotl <= 0)
            {
                return false;
            }

            try
            {
                unitOfWork.CreateTransaction();
                var sachthanhly = unitOfWork.Context.KhoSachThanhLies.FirstOrDefault(p => p.masachkho == x.masachkho);

                if (sachthanhly != null)
                {
                    // Cập nhật KhoSachThanhLy
                    sachthanhly.soluongkhotl += x.soluongkhotl;
                    unitOfWork.Context.KhoSachThanhLies.AddOrUpdate(sachthanhly);

                    // Cập nhật Sach
                    var sach = unitOfWork.Context.Saches.FirstOrDefault(s => s.MaSach == x.masachkho);
                    if (sach != null)
                    {
                        sach.SoLuongHIENTAI -= x.soluongkhotl;
                        unitOfWork.Context.Saches.AddOrUpdate(sach);
                    }

                    unitOfWork.Commit();
                    unitOfWork.Save();
                    return true;
                }
                else
                {
                   

                    var newsachtl = new KhoSachThanhLy()
                    {
                        masachkho = x.masachkho,
                        soluongkhotl = x.soluongkhotl,
                    };

                    unitOfWork.Context.KhoSachThanhLies.Add(newsachtl);

                    // Cập nhật Sach
                    var sach = unitOfWork.Context.Saches.FirstOrDefault(s => s.MaSach == x.masachkho);
                    if (sach != null)
                    {
                        sach.SoLuongHIENTAI -= x.soluongkhotl;
                        unitOfWork.Context.Saches.AddOrUpdate(sach);
                    }

                    unitOfWork.Commit();
                    unitOfWork.Save();
                    return true;
                }
            }
            catch (Exception ex)
            {
                unitOfWork.Rollback();
                Console.WriteLine($"Lỗi: {ex.Message}");
                return false;
            }
            finally
            {
                unitOfWork.Dispose();
            }
        }

        public void Update(KhoSachThanhLy obj)
        {
            throw new NotImplementedException();
        }

     
     
    }
}