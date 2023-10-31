using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebQuanLyThuVien.Models;

namespace WebQuanLyThuVien.Areas.Admin.Controllers
{
    public class HomeController : Controller
    {
        QuanLyThuVienEntities db = new QuanLyThuVienEntities();

        // GET: Admin/Home
        public ActionResult Index()
        {
            var items = db.NhanViens.ToList();
            return View(items);
        }
    }
}