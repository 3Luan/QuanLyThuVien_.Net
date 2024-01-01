using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Cryptography.Xml;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.WebPages;
using WebQuanLyThuVien.App_Start;
using WebQuanLyThuVien.Models;

namespace WebQuanLyThuVien.Controllers
{
    public class UserController : Controller
    {

        QuanLyThuVienEntities db = new QuanLyThuVienEntities();

        // GET: User
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(string uname, string pswd)
        {
            mapTaiKhoan map = new mapTaiKhoan();
            var user = map.TimKiem(uname, pswd);

            // 1. Co thi sang trang chu
            if(user != null)
            {
                SessionConfig.SetUser(user);
                Session["shared_SDT"] = user.SDT;

                TempData["user_name"] = user.SDT;
                TempData["user_password"] = user.PASSWORD_DG;

                return RedirectToAction("Index", "Home");
            }
                // 2. Khong co, quay lai trang login, bao loi
                ViewBag.error = "* Tên đăng nhập hoặc mật khẩu sai";
                return View();
        }

        public ActionResult DangKy()
        {
            return View();
        }

        [HttpPost]
        public ActionResult DangKyTaiKhoan(string email)
        {
            // Tạo biến random ra mã xác thực
            Random rd = new Random();
            // Tạo số ngẫu nhiên có 6 chữ số
            int randomNumber = rd.Next(100000, 1000000);

            TempData["OTP"] = randomNumber.ToString();

            // Gửi mail cho khách hàng
            string mailDangKy = System.IO.File.ReadAllText(Server.MapPath("~/Content/mailDangKy.html"));
            mailDangKy = mailDangKy.Replace("{{MaCode}}", randomNumber.ToString());
            WebQuanLyThuVien.Common.CommonController.SendEmail("Thư viện ABC", "Xác nhận tài khoản", mailDangKy.ToString(), email);

            return Json(new { success = true});

        }

        [HttpPost]
        public ActionResult XacNhanDangKyTaiKhoan(string hoTen, string email, string sdt, string matKhau, string OTPInput)
        {
            if (OTPInput == TempData["OTP"].ToString())
            {
                mapTaiKhoan map = new mapTaiKhoan();
                LOGIN_DG data = new LOGIN_DG()
                {
                    Email = email,
                    HoTen = hoTen,
                    PASSWORD_DG = matKhau,
                    SDT = sdt,
                };

                if (map.ThemMoi(data) == true)
                {
                    // Chuyển hướng đến hành động "Index" của controller "User"
                    return Json(new { success = true, message = "Đăng ký thành công!" });
                }
                else
                {
                    // Trả về phản hồi JSON nếu cần
                    return Json(new { success = false, message = "Đăng ký thất bại!" });
                }
            }
            else
            {
                return Json(new { success = false, message = "OTP không chính xác!" });

            }


        }


        public ActionResult Logout()
        {
            SessionConfig.SetUser(null);
            Session["shared_SDT"] = null;
            ListSachMuon.listSachMuon.Clear();
            return RedirectToAction("Index","Home");
        }

        public ActionResult UpdatePassWord()
        {
            return View();
        }

        [HttpPost]
        
        public ActionResult UpdatePassWord(string uname)
        {
            // lấy user để lấy ra thuộc thính email
            LOGIN_DG lg = db.LOGIN_DG.Find(uname);

            // tìm kếm user dựa trên uname nhập vào
            var user = db.LOGIN_DG.FirstOrDefault(u => u.SDT == uname);


            if (user != null)
            {
                TempData["uname_UpdatePass"] = uname;

                // Tạo biến random ra mã xác thực
                Random rd = new Random();
                // Tạo số ngẫu nhiên có 6 chữ số
                int randomNumber = rd.Next(100000, 1000000);

                TempData["OTP"] = randomNumber.ToString();

                // Gửi mail cho khách hàng
                string mailDangKy = System.IO.File.ReadAllText(Server.MapPath("~/Content/mailLayLaiMatKhau.html"));
                mailDangKy = mailDangKy.Replace("{{MaCode}}", randomNumber.ToString());
                WebQuanLyThuVien.Common.CommonController.SendEmail("Thư viện ABC", "Xác nhận tài khoản", mailDangKy.ToString(), lg.Email);

                return Json(new { success = true });
            }
            else
            {
                return Json(new { success = false });

            }
        }
        public ActionResult XacNhanUpdatePassword(string OTPInput)
        {
            if (OTPInput == TempData["OTP"].ToString())
            {
                return Json(new { success = true });
            }
            else
            {
                return Json(new { success = false });

            }

        }

        public ActionResult NewPassword()
        {
            return View();
        }

        [HttpPost]

        public ActionResult NewPassword(string passwordInput)
        {
            var userName = TempData["uname_UpdatePass"].ToString();

            var user = db.LOGIN_DG.Find(userName);

            if (user != null)
            {
                user.PASSWORD_DG = passwordInput;

                db.SaveChanges();

                return Json(new { success = true });
            }
            else
            {
                return Json(new { success = false });

            }

        }



        public ActionResult History()
        {
            
                var sessionValue = Session["shared_SDT"].ToString(); // Convert to string or appropriate type

                var data = db.DkiMuonSaches.Where(m => m.SDT == sessionValue).ToList();

                if (data.Count > 0)
                {
                    return View(data);
                }
                else
                {
                    // Handle when there is no data
                    ViewBag.Message = "Không có dữ liệu.";
                    return View();
                }
            
            
        }

        // Tạo đối tượng để trả về view tìm kiếm
        public class ChitietDKDetailsResponse
        {
            public string tenSach { get; set; }
            public ChiTietDk chiTietDk { get; set; }
        }

        [HttpPost]
        public ActionResult GetDetails(int maDK)
        {
            // lấy ra danh sách chi tiết đăng ký theo mã đăng ký nhập vào
            var details = db.ChiTietDks.Where(d => d.MaDK == maDK).ToList();

            // tạo một danh sách chứa chi tiết đăng ký không có các liên kết khoá ngoại

            List<ChitietDKDetailsResponse> chiTietDkList2 = new List<ChitietDKDetailsResponse>();


            foreach (var d in details)
            {
                ChitietDKDetailsResponse chiTietDkList = new ChitietDKDetailsResponse();

                var chiTietDk = new ChiTietDk()
                {
                    MaDK = d.MaDK,
                    MaSach = d.MaSach,
                    Soluongmuon = d.Soluongmuon
                };

                chiTietDkList.tenSach = db.Saches.Find(d.MaSach).TenSach;
                chiTietDkList.chiTietDk = chiTietDk;

                chiTietDkList2.Add(chiTietDkList);
            }

            return Json(new { success = true, details = chiTietDkList2 });
        }

        public ActionResult HuyDon(int maDK)
        {
            var madkToUpdate = maDK; // Thay thế 123 bằng mã đăng ký cụ thể cần cập nhật

            // Lấy bản ghi cần cập nhật từ cơ sở dữ liệu
            var recordToUpdate = db.DkiMuonSaches.FirstOrDefault(d => d.MaDK == madkToUpdate);

            if (recordToUpdate != null)
            {
                // Cập nhật trạng thái ở đây
                recordToUpdate.Tinhtrang = 3; // Thay thế 1 bằng giá trị trạng thái mới

                // Lưu thay đổi vào cơ sở dữ liệu
                db.SaveChanges();
            }
            return Json(new { success = true, message = "Đã hủy đơn thành công." });
        }

    }
}