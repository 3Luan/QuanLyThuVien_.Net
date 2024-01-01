 using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebQuanLyThuVien.Models
{
    public class mapTaiKhoan
    {
        QuanLyThuVienEntities db = new QuanLyThuVienEntities();
        public LOGIN_DG TimKiem(string username, string password)
        {
            var user = db.LOGIN_DG.Where(m=>m.SDT == username && m.PASSWORD_DG == password).ToList();

            if(user.Count() > 0 )
            {
                return user[0];
            }
            else
            {
                return null;
            }
        }

        // Tao tai khoan moi
        public bool ThemMoi(LOGIN_DG lg)
        {
            try
            {
                db.LOGIN_DG.Add(lg);

                // Luu vao database
                db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }

        }
    }
}