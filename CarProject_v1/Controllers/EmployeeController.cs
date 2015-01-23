using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace CarProject_v1.Controllers
{
    [Authorize]
    public class EmployeeController : Controller
    {
        //
        // GET: /Employee/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult signOut()
        {
            FormsAuthentication.SignOut();
            return Redirect("~/Guest/Index");
        }

        [Authorize(Roles = "Employee")]
        public ActionResult EmployeeIndex()
        {
            ViewBag.Message = "This can be viewed only by users in Employee role only";
            return View();
        }
    }
}
