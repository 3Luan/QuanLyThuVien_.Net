using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using WebQuanLyThuVien.Services;

namespace WebQuanLyThuVien.Areas.Admin.Controllers
{
    public class AccountController : Controller
    {
        // GET: Admin/Account
        NhanVienService _nhanVienService = new NhanVienService();

        public ActionResult Index()
        {
            if (Session["user"] == null)
                return RedirectToAction("Login");
            else
                return View();
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(string username, string password)
        {
            // check database
            var login = _nhanVienService.Login(username, password);
            if (login != null)
            {
                //Session["user"] = login.HoTenNV +" ("+ login.ChucVu+")";
                Session["user"] = login.HoTenNV;
                Session["chucvu"] = login.ChucVu;
                Session["manv"] = login.MaNV;

                return RedirectToAction("Index", "Home");
            }
            else
            {
                TempData["error"] = "Tài khoản hoặc mật khẩu không chính xác!";
                return View();
            }
        }

        public ActionResult Logout()
        {
            // Xóa session
            Session.Remove("user");

            // Xóa session from authent
            FormsAuthentication.SignOut();

            return RedirectToAction("Login");
        }
    }
}