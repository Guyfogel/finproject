using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using DAL;
using Models;
using System.Globalization;
using System.Data.Entity.Validation;
using System.Diagnostics;

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

        public ActionResult GetOrderForReceipt(int orderID)
        {
            //if (!Request.IsAjaxRequest())
            //{
            //    return null;
            //}
            CarRepository repo = new CarRepository();
            object order = repo.GetOrderForReceipt(orderID);
            return Json(order, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ReturnCar()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ReturnCar(int orderID, DateTime actualreturndate)
        {
            if (!Request.IsAjaxRequest())
            {
                return null;
            }
            CarRepository repo = new CarRepository();
            Orders order = repo.GetOrderbyID(orderID);
            decimal fp = repo.ReturnCar(order, actualreturndate);


            return Json("/Employee/Receipt?orderID="+order.OrderID+"&finalprice="+fp);
        }

        public ActionResult Receipt(int orderID, decimal finalprice)
        {
            //if (!Request.IsAjaxRequest())
            //{
            //    return null;
            //}
            CarRepository repo = new CarRepository();
            
            Receipt receipt = new Receipt() { OrderID = orderID, FinalPrice = finalprice };
            return View(receipt);
        }
        

        [Authorize]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("index", "Guest");
        }

        [Authorize(Roles = "Employee")]
        public ActionResult EmployeeIndex()
        {
            ViewBag.Message = "This can be viewed only by users in Employee role only";
            return View();
        }
    }
}
