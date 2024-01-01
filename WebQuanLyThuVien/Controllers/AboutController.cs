using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using WebQuanLyThuVien.Models;

namespace WebQuanLyThuVien.Controllers
{
    public class AboutController : Controller
    {
        QuanLyThuVienEntities db = new QuanLyThuVienEntities();

        // GET: About

        public ActionResult Index()
        {
            return View();
        }

    }
}