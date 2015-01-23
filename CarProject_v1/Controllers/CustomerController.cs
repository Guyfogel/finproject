using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CarProject_v1.Controllers
{
    [Authorize]
    public class CustomerController : Controller
    {
        //
        // GET: /Customer/

        public ActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = "Customer")]
        public ActionResult CustomerIndex()
        {
            ViewBag.Message = "This can be viewed only by users in Customer role only";
            return View();
        }

    }
}
