using DAL;
using Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

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

        [Authorize]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("index", "Guest");
        }

        [Authorize(Roles = "Customer")]
        public ActionResult CustomerIndex()
        {
            ViewBag.Message = "This can be viewed only by users in Customer role only";
            return View();
        }

        public ActionResult AddOrder()
        
        {
            return View();
        }

        public ActionResult GetCarType(string carStr)
        {
            if (!Request.IsAjaxRequest())
            {
                return null;
            }
            CarRepository repo = new CarRepository();
            CarTypes cartype = new CarTypes();
            string[] cartypeStrArr = carStr.Split(' ');
            cartype = repo.GetCarType(cartypeStrArr[0], cartypeStrArr[1]);
            return Json(cartype, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult AddOrder(string carStr, string lenddate, string returndate)
        {
            CarRepository repo = new CarRepository();
            try
            {
                Cars car = new Cars();
                string[] cartypeStrArr = carStr.Split(' ');
                car = repo.GetCarByCartype(cartypeStrArr[0], cartypeStrArr[1]);
                Users user = new Users();
                HttpCookie hc = Request.Cookies[FormsAuthentication.FormsCookieName];
                user = repo.GetUser(FormsAuthentication.Decrypt(hc.Value).Name);
                DateTime Ldate = DateTime.ParseExact(lenddate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                DateTime Rdate = DateTime.ParseExact(returndate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                Orders order = new Orders() { Car = car, CarID = car.CarID, UserID = user.UserID, User = user, LendDate = Ldate, ReturnDate = Rdate };
                repo.AddOrder(order);
                return Json(new { Status = "OK", Car = car });
            }
            catch (DbEntityValidationException dbEx)
            {
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        Trace.TraceInformation("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                    }

                }
                return Json(new { Status = "Fail" });
            }
            catch (Exception ex)
            {
                return Json(new { Status = "Fail" });
            }
        }

        public ActionResult ViewOrders()
        {
            return View();
        }
        public ActionResult GetAllOrdersByUser()
        {
            //if (!Request.IsAjaxRequest())
            //{
            //    return null;
            //}

            CarRepository repo = new CarRepository();
            HttpCookie hc = Request.Cookies[FormsAuthentication.FormsCookieName];
            IEnumerable<object> orders = repo.GetAllOrdersByUsername(FormsAuthentication.Decrypt(hc.Value).Name);
            return Json(orders, JsonRequestBehavior.AllowGet);
        }


        public ActionResult GetAllCars()
        {
            if (!Request.IsAjaxRequest())
            {
                return null;
            }
            CarRepository repo = new CarRepository();
            IEnumerable<object> Cars = repo.GetAllCars();
            return Json(Cars, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetAllCarTypes()
        {
            if (!Request.IsAjaxRequest())
            {
                return null;
            }
            CarRepository repo = new CarRepository();
            IEnumerable<CarTypes> CarTypes = repo.GetAllCarTypes();
            return Json(CarTypes, JsonRequestBehavior.AllowGet);
        }

    }
}
