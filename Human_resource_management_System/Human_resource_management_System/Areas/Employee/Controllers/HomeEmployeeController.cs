using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using Human_resource_management_System.Models;

namespace Human_resource_management_System.Areas.Employee.Controllers
{
    [Authorize(Roles = "NhanVien")]
    public class HomeEmployeeController : Controller
    {
        private ModelDbContext db = new ModelDbContext();

        // GET: Employee/HomeEmployee
        public ActionResult Index()
        {
            var username = User.Identity.Name;
            var account = db.TaiKhoans.FirstOrDefault(t => t.tenDangNhap == username);
            return View(account?.NhanVien);
        }

        // GET: Employee/HomeEmployee/Info
        public ActionResult Info()
        {
            // Get current username from the authenticated session
            string username = User.Identity.Name;

            // Find the account and associated employee details
            var account = db.TaiKhoans.FirstOrDefault(t => t.tenDangNhap == username);

            if (account == null || account.NhanVien == null)
            {
                // In case of data inconsistency or if not found
                return HttpNotFound("Không tìm thấy thông tin nhân viên.");
            }

            // Pass the NhanVien model to the view
            return View(account.NhanVien);
        }
        // GET: Employee/HomeEmployee/ChangePassword
        public ActionResult ChangePassword()
        {
            return View();
        }

        // POST: Employee/HomeEmployee/ChangePassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangePassword(Human_resource_management_System.Models.Form.ChangePassword model)
        {
            if (ModelState.IsValid)
            {
                string username = User.Identity.Name;
                var account = db.TaiKhoans.FirstOrDefault(t => t.tenDangNhap == username);

                if (account == null)
                {
                    return HttpNotFound();
                }

                if (account.matKhau != model.MatKhauHienTai)
                {
                    ModelState.AddModelError("", "Mật khẩu hiện tại không đúng.");
                    return View(model);
                }

                account.matKhau = model.MatKhauMoi;
                db.SaveChanges();

                TempData["SuccessMessage"] = "Đổi mật khẩu thành công!";
                return RedirectToAction("Info");
            }
            return View(model);
        }

        public ActionResult Schedule()
        {
            var username = User.Identity.Name;
            var account = db.TaiKhoans
                .Include(t => t.NhanVien)
                .FirstOrDefault(t => t.tenDangNhap == username);

            if (account == null)
            {
                return HttpNotFound("Không tìm thấy tài khoản đăng nhập.");
            }

            // Prefer foreign key on TaiKhoan to avoid relying on navigation mapping.
            var employeeId = account.maNhanVien;
            if (string.IsNullOrWhiteSpace(employeeId))
            {
                employeeId = account.NhanVien?.maNhanVien;
            }

            if (string.IsNullOrWhiteSpace(employeeId))
            {
                return HttpNotFound("Tài khoản chưa gắn với mã nhân viên.");
            }

            var employeeName = account.NhanVien?.hoTen;
            if (string.IsNullOrWhiteSpace(employeeName))
            {
                employeeName = db.NhanViens
                    .Where(n => n.maNhanVien == employeeId)
                    .Select(n => n.hoTen)
                    .FirstOrDefault();
            }

            var schedules = db.LichLamViecs
                .Include(l => l.CaLamViec)
                .Where(l => l.maNhanVien == employeeId)
                .OrderBy(l => l.ngayLamViec)
                .ThenBy(l => l.maCa)
                .ToList();

            ViewBag.EmployeeName = string.IsNullOrWhiteSpace(employeeName) ? employeeId : employeeName;
            ViewBag.EmployeeId = employeeId;
            return View(schedules);
        }

        public ActionResult Salary()
        {
            var username = User.Identity.Name;
            var account = db.TaiKhoans
                .Include(t => t.NhanVien)
                .FirstOrDefault(t => t.tenDangNhap == username);

            if (account == null)
            {
                return HttpNotFound("Không tìm thấy tài khoản đăng nhập.");
            }

            var employeeId = account.maNhanVien;
            if (string.IsNullOrWhiteSpace(employeeId))
            {
                employeeId = account.NhanVien?.maNhanVien;
            }

            if (string.IsNullOrWhiteSpace(employeeId))
            {
                return HttpNotFound("Tài khoản chưa gắn với mã nhân viên.");
            }

            var employeeName = account.NhanVien?.hoTen;
            if (string.IsNullOrWhiteSpace(employeeName))
            {
                employeeName = db.NhanViens
                    .Where(n => n.maNhanVien == employeeId)
                    .Select(n => n.hoTen)
                    .FirstOrDefault();
            }

            var salaries = db.BangLuongs
                .Where(b => b.maNhanVien == employeeId)
                .OrderByDescending(b => b.nam)
                .ThenByDescending(b => b.thang)
                .ToList();

            ViewBag.EmployeeName = string.IsNullOrWhiteSpace(employeeName) ? employeeId : employeeName;
            ViewBag.EmployeeId = employeeId;
            return View(salaries);
        }
    }
}