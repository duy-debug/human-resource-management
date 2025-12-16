using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Human_resource_management_System.Models;
using System.Web.Security;

namespace Human_resource_management_System.Controllers
{
    public class AccountController : Controller
    {
        ModelDbContext db = new ModelDbContext();

        // GET: Account
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(Human_resource_management_System.Models.Form.Login model)
        {
            if (ModelState.IsValid)
            {
                var user = db.TaiKhoans.FirstOrDefault(u => u.tenDangNhap == model.tenDangNhap && u.matKhau == model.matKhau);
                if (user != null)
                {
                    if (user.trangThai == false)
                    {
                        ModelState.AddModelError("", "Tài khoản của bạn đã bị khóa.");
                        return View(model);
                    }

                    var ticket = new FormsAuthenticationTicket(
                        1, 
                        user.tenDangNhap, 
                        DateTime.Now, 
                        DateTime.Now.AddMinutes(2880), 
                        false, 
                        user.vaiTro // Storing role in UserData
                    );
                    string encryptedTicket = FormsAuthentication.Encrypt(ticket);
                    var authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
                    Response.Cookies.Add(authCookie);

                    if (user.vaiTro == "Admin")
                    {
                        return RedirectToAction("Index", "HomeAdmin", new { area = "Admin" });
                    }
                    else if (user.vaiTro == "NhanVien")
                    {
                        return RedirectToAction("Index", "HomeEmployee", new { area = "Employee" });
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Tên đăng nhập hoặc mật khẩu không chính xác.");
                }
            }
            return View("Index", model);
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Account");
        }
    }
}