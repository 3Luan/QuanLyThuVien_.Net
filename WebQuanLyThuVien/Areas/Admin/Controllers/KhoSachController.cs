using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebQuanLyThuVien.Services;

namespace WebQuanLyThuVien.Areas.Admin.Controllers
{
    public class KhoSachController : Controller
    {
        SachService _sachService = new SachService();

        // GET: Admin/KhoSach
        public ActionResult Index()
        {
            if (Session["user"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else if (Session["chucvu"].ToString().ToLower() == "thuthu")
            {
                return RedirectToAction("loiphanquyen", "phanquyen");
            }
            else
            {
                ViewData["Sach"] = _sachService.GetAll();
                return View();
            }
        }
    }
}