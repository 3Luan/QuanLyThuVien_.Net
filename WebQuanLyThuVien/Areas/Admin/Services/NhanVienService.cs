using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using WebQuanLyThuVien.Areas.Admin.Data;
using WebQuanLyThuVien.Interfaces;
using WebQuanLyThuVien.Interfaces.Services;
using WebQuanLyThuVien.Models;
using WebQuanLyThuVien.Repository;

namespace WebQuanLyThuVien.Services
{
    /// <summary>
    /// Service dùng để sử lý logic, kiểm tra đầu vào.....
    /// 
    /// VD: Thêm 1 nhân viên, giao diện người dùng truyền data sáng controller => kiểm tra thông tin đúng hết mới gọi repo
    /// </summary>
    public class NhanVienService : INhanVienService
    {
        private INhanVienRepository _nhanVienRepository;

        private UnitOfWork<QuanLyThuVienEntities> unitOfWork = new UnitOfWork<QuanLyThuVienEntities>();

        public NhanVienService()
        {
            _nhanVienRepository = new NhanVienRepository(unitOfWork);
        }


        /// <summary>
        /// KHi xoá chú ý có thể xảy ra lỗi liên quan khoá ngoại nằm ở bảng khác
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string Delete(int id)
        {
            // check xem nếu thành công hình như sẽ > 0 trả return thông báo cho UI người dùng
            var isSuccess = _nhanVienRepository.Delete(id);
            if (isSuccess > 0)
                return "Xoá thành công";

            return "Xoá không thành công";
        }

        public IEnumerable<NhanVien> GetAll()
        {
            return _nhanVienRepository.GetAll();
        }

        public IEnumerable<DTO_NhanVien_LoginNV> GetAllNhanVien()
        {
            var listNhanVien =
                (from NhanVien in unitOfWork.Context.NhanViens
                 select new DTO_NhanVien_LoginNV
                 {
                     MaNV = NhanVien.MaNV,
                     HoTenNV = NhanVien.HoTenNV,
                     GioiTinh = NhanVien.GioiTinh,
                     DiaChi = NhanVien.DiaChi,
                     NgaySinh = (DateTime)NhanVien.NGAYSINH,
                     SDT = NhanVien.SDT,
                     ChucVu = NhanVien.ChucVu,
                 }
                 ).ToList();
            return listNhanVien;
        }

        public DTO_NhanVien_LoginNV GetById(int id)
        {
            try
            {
                var DTO_NhanVien_LoginNV = (
                    from NhanVien in unitOfWork.Context.NhanViens
                    join LOGIN_NV in unitOfWork.Context.LOGIN_NV
                    on NhanVien.MaNV equals LOGIN_NV.MANV
                    where NhanVien.MaNV == id
                    select new DTO_NhanVien_LoginNV
                    {
                        MaNV = NhanVien.MaNV,
                        HoTenNV = NhanVien.HoTenNV,
                        NgaySinh = (DateTime)NhanVien.NGAYSINH,
                        GioiTinh = NhanVien.GioiTinh,
                        DiaChi = NhanVien.DiaChi,
                        ChucVu = NhanVien.ChucVu,
                        SDT = NhanVien.SDT,
                        Username = LOGIN_NV.USERNAME_NV,
                        Password = LOGIN_NV.PASSWORD_NV,
                    }).FirstOrDefault();

                return DTO_NhanVien_LoginNV;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetById: {ex.Message}");
                throw;
            }
        }

        public DTO_NhanVien_LoginNV Login(string username, string password)
        {
            // Chưa mã hóa password
            var nhanVien =
                 (from nv in unitOfWork.Context.NhanViens
                  join lg in unitOfWork.Context.LOGIN_NV
                     on nv.MaNV equals lg.MANV
                  where lg.USERNAME_NV == username && password == lg.PASSWORD_NV
                  select new DTO_NhanVien_LoginNV
                  {
                      MaNV = nv.MaNV,
                      HoTenNV = nv.HoTenNV,
                      SDT = nv.SDT,
                      ChucVu = nv.ChucVu,
                      DiaChi = nv.DiaChi
                  }).FirstOrDefault();
            return nhanVien;

            // Đã mã hóa password
            //var login = (
            //    from nv in unitOfWork.Context.NhanViens
            //    join lg in unitOfWork.Context.LOGIN_NV on nv.MaNV equals lg.MANV
            //    where lg.USERNAME_NV.Equals(username)
            //    select new
            //    {
            //        passwordHash = lg.PASSWORD_NV,
            //        NhanVien = nv
            //    }
            //).FirstOrDefault();

            //if (login != null)
            //{
            //    var verify = BCrypt.Net.BCrypt.Verify(password, login.passwordHash);
            //    if (verify)
            //    {
            //        var nhanVien = new DTO_NhanVien_LoginNV
            //        {
            //            MaNV = login.NhanVien.MaNV,
            //            HoTenNV = login.NhanVien.HoTenNV,
            //            SDT = login.NhanVien.SDT,
            //            ChucVu = login.NhanVien.ChucVu,
            //            DiaChi = login.NhanVien.DiaChi
            //        };

            //        return nhanVien;
            //    }
            //    else
            //        return null;
            //}
            //else
            //    return null;
        }


        public NhanVien GetBySDT(string sdt)
        {
            var data = _nhanVienRepository.GetBySDT(sdt);
            // check null gì đó ....
            return data;
        }

        public void Insert(NhanVien obj)
        {
            var isSuccess = _nhanVienRepository.Insert(obj);
        }

        public void Update(NhanVien nhanvien)
        {
            // check xem nếu thành công hình như sẽ > 0 trả return thông báo cho UI người dùng
            var isSuccess = _nhanVienRepository.Update(nhanvien);
        }

        public void ThemNhanVien(DTO_NhanVien_LoginNV obj)
        {
            try
            {
                var existingSDT = unitOfWork.Context.NhanViens.FirstOrDefault(nv => nv.SDT == obj.SDT);

                if (existingSDT != null)
                {
                    throw new Exception("Số điện thoại đã tồn tại.");
                }
                else
                {
                    // Tạo một đối tượng nhan viên mới
                    var newNhanVien = new NhanVien
                    {
                        HoTenNV = obj.HoTenNV,
                        SDT = obj.SDT,
                        GioiTinh = obj.GioiTinh,
                        NGAYSINH = obj.NgaySinh,
                        DiaChi = obj.DiaChi,
                        ChucVu = obj.ChucVu,
                    };

                    unitOfWork.Context.NhanViens.Add(newNhanVien);
                    unitOfWork.Save(); // Lưu các thay đổi vào cơ sở dữ liệu

                    // Mã hóa mật khẩu với bcrypt
                    string hashedPassword = BCrypt.Net.BCrypt.HashPassword(obj.Password, 12);

                    // Tạo một đối tượng loginNV mới
                    var newLoginNV = new LOGIN_NV
                    {
                        MANV = newNhanVien.MaNV,
                        USERNAME_NV = obj.Username,
                        PASSWORD_NV = hashedPassword,
                    };

                    unitOfWork.Context.LOGIN_NV.Add(newLoginNV);
                    unitOfWork.Save(); // Lưu các thay đổi vào cơ sở dữ liệu
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi trong ThemNhanVien: {ex.Message}");
                throw;
            }
        }

        public void UpdateThongTinNhanVien(DTO_NhanVien_LoginNV obj)
        {
            try
            {
                var existingSDT = unitOfWork.Context.NhanViens.FirstOrDefault(nv => nv.SDT == obj.SDT && nv.MaNV != obj.MaNV);
                var existingUsername = unitOfWork.Context.LOGIN_NV.FirstOrDefault(login => login.USERNAME_NV == obj.Username && login.MANV != obj.MaNV);

                if (existingSDT != null)
                {
                    throw new Exception("Số điện thoại đã tồn tại.");
                }
                else
                {
                    if (existingUsername != null)
                    {
                        throw new Exception("Username đã tồn tại.");
                    }
                    else
                    {
                        var nhanVien = unitOfWork.Context.NhanViens.FirstOrDefault(t => t.MaNV == obj.MaNV);
                        nhanVien.HoTenNV = obj.HoTenNV;
                        nhanVien.SDT = obj.SDT;
                        nhanVien.NGAYSINH = obj.NgaySinh;
                        nhanVien.GioiTinh = obj.GioiTinh;
                        nhanVien.DiaChi = obj.DiaChi;
                        nhanVien.ChucVu = obj.ChucVu;

                        var login = unitOfWork.Context.LOGIN_NV.FirstOrDefault(t => t.MANV == obj.MaNV);

                        if (obj.Password.Trim() == "")
                        {
                            login.USERNAME_NV = obj.Username;
                            //login.PASSWORD_NV = obj.Password;
                            // Lưu thay đổi vào cơ sở dữ liệu
                            unitOfWork.Context.SaveChanges();
                        }
                        else
                        {
                            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(obj.Password, 12);

                            login.USERNAME_NV = obj.Username;
                            login.PASSWORD_NV = hashedPassword;

                            unitOfWork.Context.SaveChanges();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in Update: {ex.Message}");
                throw;
            }
        }

    }
}
