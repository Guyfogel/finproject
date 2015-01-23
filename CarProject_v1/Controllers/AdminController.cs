using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DAL;
using Models;
using System.Globalization;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Web.Security;

namespace CarProject_v1.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        //
        // GET: /Admin/

        public ActionResult AddUser()
        {
            return View();
        }

        public ActionResult GetAllUsers()
        {
            if (!Request.IsAjaxRequest())
            {
                return null;
            }
            CarRepository repo = new CarRepository();
            IEnumerable<Users> Users = repo.GetAllUsers();
            return Json(Users, JsonRequestBehavior.AllowGet);
        }
        public ActionResult AddOrder()
        {
            return View();
        }
        [HttpPost]
        public ActionResult AddOrder(string carStr, string lenddate, string returndate, string username)
        {
            CarRepository repo = new CarRepository();
            try
            {
                Cars car = new Cars();
                string[] cartypeStrArr = carStr.Split(' ');
                car = repo.GetCarByCartype(cartypeStrArr[0], cartypeStrArr[1]);
                Users user = new Users();
                user = repo.GetUser(username);
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
        public ActionResult GetAllOrders()
        {
            //if (!Request.IsAjaxRequest())
            //{
            //    return null;
            //}
            CarRepository repo = new CarRepository();
            IEnumerable<object> orders = repo.GetAllOrders();
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

        //public ActionResult GetCarTypes(string Manufacturer)
        //{
        //    if (!Request.IsAjaxRequest())
        //    {
        //        return null;
        //    }
        //    CarRepository repo = new CarRepository();
        //    IEnumerable<CarTypes> CarTypes = repo.GetCarTypes(Manufacturer,null);
        //    return Json(CarTypes, JsonRequestBehavior.AllowGet);
        //}

        public ActionResult GetAllLocations()
        {
            if (!Request.IsAjaxRequest())
            {
                return null;
            }
            CarRepository repo = new CarRepository();
            IEnumerable<Locations> Locations = repo.GetAllLocations();
            return Json(Locations, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult AddUser(string username, string password, string repeatpassword, string name, string email, Genders gender, string Personnumber, byte[] pic)
        {
            CarRepository repo = new CarRepository();
            try
            {
                if (password != repeatpassword) throw new Exception();
                Users user = new Users() { Username = username, Password = password, Name = name, Email = email, PersonNum = int.Parse(Personnumber), Gender = gender, Pic = pic };
                repo.AddUser(user);
                return Json(new { Status = "OK", User = user });
            }
            catch (Exception)
            {
                return Json(new { Status = "Fail" });
            }
        }
        //[HttpPost]
        //public ActionResult AddUser(Users user)
        //{
        //    CarRepository repo = new CarRepository();
        //    try
        //    {
        //        repo.AddUser(user);
        //        return Json(new { Status = "OK", User = user });
        //    }
        //    catch (Exception)
        //    {
        //        return Json(new { Status = "Fail" });
        //    }
        //}

        
        //public ActionResult AddCar(Cars car)
        //{
        //    CarRepository repo = new CarRepository();
        //    try
        //    {
        //        repo.AddCar(car);
        //        return Json(new { Status = "OK", Car = car });
        //    }
        //    catch (Exception)
        //    {
        //        return Json(new { Status = "Fail" });
        //    }
        //}
        [HttpPost]
        public ActionResult AddCar(string cartypeStr,string kilometrage,string locationName)
        {
            CarRepository repo = new CarRepository();
            try
            {
                CarTypes cartype = new CarTypes();
                string[] cartypeStrArr = cartypeStr.Split(' ');
                cartype = repo.GetCarType(cartypeStrArr[0], cartypeStrArr[1]);
                Locations location = new Locations();
                location = repo.GetLocation(locationName);
                Cars car = new Cars() { CarType = cartype, CarTypeID= cartype.CarTypeID, isAvailable = true, Kilometrage = int.Parse(kilometrage), Location = location, LocationID=location.LocationID };
                repo.AddCar(car);
                return Json(new { Status = "OK", Car = car });
            }
            catch (Exception)
            {
                return Json(new { Status = "Fail" });
            }
        }

        [HttpPost]
        public ActionResult AddCarType(CarTypes cartype)
        {
            CarRepository repo = new CarRepository();
            try
            {
                repo.AddCarType(cartype);
                return Json(new { Status = "OK", CarType = cartype });
            }
            catch (Exception)
            {
                return Json(new { Status = "Fail" });
            }
        }

        public ActionResult AddCar()
        {
            return View();
        }

        public ActionResult AddCarType()
        {
            return View();
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult signOut()
        {
            FormsAuthentication.SignOut();
            return Redirect("~/Guest/Index");
        }

        [Authorize(Roles = "Admin")]
        public ActionResult AdminIndex()
        {
            ViewBag.Message = "This can be viewed only by users in Admin role only";
            return View();
        }
    }
}
