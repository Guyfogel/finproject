using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DAL;
using Models;

namespace CarProject_v1.Controllers
{
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
        public ActionResult AddUser(Users user)
        {
            CarRepository repo = new CarRepository();
            try
            {
                repo.AddUser(user);
                return Json(new { Status = "OK", User = user });
            }
            catch (Exception)
            {
                return Json(new { Status = "Fail" });
            }
        }

        [HttpPost]
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
    }
}
