using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using human_resource_management.Filters;

namespace human_resource_management.Areas.Employee.Controllers
{
<<<<<<< HEAD
    [RoleAuthorize("nhanvien")]
=======
    [RoleAuthorize("Nhân viên")]
>>>>>>> ddff4f5123cbfdd5d8dc84f559dfb2190309d627
    public class HomeController : Controller
    {
        // GET: Employee/Home
        public ActionResult Index()
        {
            return View();
        }
    }
}