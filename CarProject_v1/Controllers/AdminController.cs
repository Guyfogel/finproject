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
using Newtonsoft.Json.Serialization;

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


            //JsonNetResult jsonNetResult = new JsonNetResult();

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
                return Json(new { Status = "Order Added", Car = car });
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
                return Json(new { Status = "Validation Error" });
            }
            catch (Exception ex)
            {
                return Json(new { Status = "Dates, Username or Car fields are empty" });
            }
        }

        public ActionResult UpdateOrder (int OrderID, string carStr, string lenddate, string returndate,string username)
        {
            CarRepository repo = new CarRepository();
            try
            {
                
                Cars car = new Cars();
                string[] cartypeStrArr = carStr.Split(' ');
                car = repo.GetCarByCartype(cartypeStrArr[0], cartypeStrArr[1]);
                Users user = new Users();
                user = repo.GetUser(username);
                Orders order = repo.UpdateOrder(OrderID, car, user, lenddate, returndate);
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
            catch (Exception)
            {
                return Json(new { Status = "Fail" });
            }
        }
        [HttpPost]
        public ActionResult RemoveOrder(string OrderID, string Username, string carStr)
        {
            CarRepository repo = new CarRepository();
            try
            {
                Orders order = new Orders();
                if (string.IsNullOrEmpty(OrderID))
                {
                    if (string.IsNullOrEmpty(carStr))
                    {
                        order = repo.GetOrderByFilter(Username, carStr, carStr);
                    }
                    else
                    {
                        string[] cartypeStrArr = carStr.Split(' ');
                        order = repo.GetOrderByFilter(Username, cartypeStrArr[0], cartypeStrArr[1]);
                    }
                }
                else 
                { 
                    order = repo.GetOrderbyID(int.Parse(OrderID)); 
                }

                repo.RemoveOrder(order);
                    return Json(new { Status = "OK", Order = order });
                
            }
            catch (Exception)
            {
                return Json(new { Status = "Fail:ID or Username missing" });
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
        
        [HttpPost]
        public ActionResult GetCarType(string carStr)
        {
            try
            {
                if (!Request.IsAjaxRequest())
                {
                    return null;
                }
                if (string.IsNullOrEmpty(carStr)) return Json(new { Status = "Car Type field is empty" });
                CarRepository repo = new CarRepository();
                CarTypes cartype = new CarTypes();
                string[] cartypeStrArr = carStr.Split(' ');
                cartype = repo.GetCarType(cartypeStrArr[0], cartypeStrArr[1]);
                return Json(cartype, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Status = "Fail" });
            }
        }

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
        
        [HttpPost]
        public ActionResult UpdateUser(int UserID, string Username, string Password, string RepeatPassword, string Name, string Email, string Gender , string Role, string Personnumber, byte[] photo )
        {
            CarRepository repo = new CarRepository();
            try
            {
                int personnum = 0;
                if (Password != RepeatPassword) return Json(new { Status = "Passwords do not match" });
                if (!string.IsNullOrEmpty(Personnumber)) personnum = int.Parse(Personnumber);
                Users user = repo.UpdateUser(UserID, Username, Password, Name, Email, Gender, Role, personnum, photo);
                return Json(new { Status = "OK", User = user });
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
            catch (Exception)
            {
                return Json(new { Status = "Fail" });
            }
        }

        [HttpPost]
        public ActionResult RemoveUser(string UserID, string Username)
        {
            CarRepository repo = new CarRepository();
            try
            {
                Users user = new Users();
                if (string.IsNullOrEmpty(UserID)) { user = repo.GetUser(Username); }
                else { user = repo.GetUserByID(int.Parse(UserID)); }
                if (!repo.isUserInOrder(user.UserID))
                {
                    repo.RemoveUser(user);
                    return Json(new { Status = "OK", User = user });
                }
                else 
                {
                    return Json(new { Status = "Error:Remove All Orders by User before removing user!" });
                }
            }
            catch (Exception)
            {
                return Json(new { Status = "Fail:ID or Username missing" });
            }
        }
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
        public ActionResult UpdateCar(int CarID,string cartypeStr,int kilometrage,string locationName,byte[]photo,bool availability)
        {
            CarRepository repo = new CarRepository();
            try
            {
                CarTypes cartype = new CarTypes();
                string[] cartypeStrArr = cartypeStr.Split(' ');
                cartype = repo.GetCarType(cartypeStrArr[0], cartypeStrArr[1]);
                Locations location = new Locations();
                location = repo.GetLocation(locationName);
                
                Cars car=repo.UpdateCar(cartype,location,kilometrage,CarID,photo,availability);
                return Json(new { Status = "OK", Car=car });
            }
            catch (Exception)
            {
                return Json(new { Status = "Fail" });
            }
        }

        [HttpPost]
        public ActionResult RemoveCar(string CarID, string cartypeStr)
        {
            CarRepository repo = new CarRepository();
            try
            {
                Cars car = new Cars();
                if (string.IsNullOrEmpty(CarID)) 
                {
                    string[] cartypeStrArr = cartypeStr.Split(' ');
                    car = repo.GetCarByCartype(cartypeStrArr[0], cartypeStrArr[1]);
                }
                else { car = repo.GetCarByID(int.Parse(CarID)); }
                if (!repo.isCarInOrder(car.CarID))
                {
                    repo.RemoveCar(car);
                    return Json(new { Status = "Car Removed", Car = car });
                }
                else
                {
                    return Json(new { Status = "Error:Remove All Orders with this car before removing car!" });
                }
            }
            catch (Exception)
            {
                return Json(new { Status = "Fail:ID or Manufacturer&model missing" });
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
        
        [HttpPost]
        public ActionResult UpdateCarType(int CarTypeID, string Manufacturer, string CarModel , string PricePerDay, string PricePerLateDay,string Gear)
        {
            CarRepository repo = new CarRepository();
            try
            {
                decimal priceperday;
                decimal priceperlateday;
                if (!string.IsNullOrEmpty(PricePerDay))
                {
                   priceperday = decimal.Parse(PricePerDay);
                }
                else
                {
                   priceperday = 0;
                }
                if (!string.IsNullOrEmpty(PricePerLateDay))
                {
                   priceperlateday = decimal.Parse(PricePerLateDay);
                }
                else
                {
                    priceperlateday = 0;
                }
                
                CarTypes cartype = repo.UpdateCarType(CarTypeID, Manufacturer, CarModel, Gear, priceperday, priceperlateday);
                
                return Json(new { Status = "OK", CarType = cartype });
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

        [HttpPost]
        public ActionResult RemoveCarType(string CarTypeID, string Manufacturer, string CarModel)
        {
            CarRepository repo = new CarRepository();
            try
            {
                CarTypes cartype = new CarTypes();
                if (string.IsNullOrEmpty(CarTypeID)) { cartype = repo.GetCarType(Manufacturer, CarModel); }
                else { cartype = repo.GetCarTypeByID(int.Parse(CarTypeID)); }
                if (!repo.isCarTypeInCar(cartype.CarTypeID))
                {
                    repo.RemoveCarType(cartype);
                    return Json(new { Status = "OK", CarType = cartype });
                }
                else
                {
                    return Json(new { Status = "Error:Remove All Cars with this car type before removing car type!" });
                }
            }
            catch (Exception)
            {
                return Json(new { Status = "Fail:ID or Manufacturer&model missing" });
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

        [Authorize]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("index", "Guest");
        }

        [Authorize(Roles = "Admin")]
        public ActionResult AdminIndex()
        {
            ViewBag.Message = "This can be viewed only by users in Admin role only";
            return View();
        }
    }
}
