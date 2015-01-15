using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Models;
using DAL;

namespace CarProject_v1.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/
        public CarRepository repo = new CarRepository();
        public HomeController()
        {
            //bookFilePath = HttpContext.Server.MapPath("/DataFiles/books.dat");
        }
        

        public ActionResult Index()
        {
            //repo.GetAllUsers();
            return View();
        }



        //public ActionResult AddUser()
        //{
        //    return View();
        //}

        //[HttpPost]
        //public ActionResult AddUser(Users user)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        repo.AddUser(user.Username, user.Password, user.Email, user.Name, user.PersonNum, user.Gender, user.Pic);
        //    }
        //    ViewBag.Message = "User registered";
        //    //userFilePath = Server.MapPath("/DataFiles/Users.dat");

        //    //using (FileStream fs = new FileStream(userFilePath, FileMode.Append))
        //    //{
        //    //    BinaryFormatter bf = new BinaryFormatter();
        //    //    bf.Serialize(fs, user);
        //    //}

        //    return View();
        //}

        //public ActionResult Index()
        //{
        //    userFilePath = Server.MapPath("/DataFiles/Users.dat");
        //    List<usersmodel> users = new List<usersmodel>();
        //    using (FileStream fs = new FileStream(userFilePath, FileMode.Open))
        //    {
        //        BinaryFormatter bf = new BinaryFormatter();
        //        while (fs.Position < fs.Length)
        //        {
        //            users.Add((usersmodel)bf.Deserialize(fs));
        //        }
        //    }
        //    return View(users);
        //}

    }
}
