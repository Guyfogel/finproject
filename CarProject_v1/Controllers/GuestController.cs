using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Models;
using DAL;
using System.IO;

namespace CarProject_v1.Controllers
{
    public class UserData
    {
        public Users userinfo { get; set; }
    }
    public class GuestController : Controller
    {
        //
        // GET: /Guest/

        public ActionResult Index()
        {
            return View();
        }
        public ActionResult AddUser()
        {
            return View();
        }
        [HttpPost]
        public ActionResult AddUser(UserSigninVM newUser, HttpPostedFileBase pic)
        {
            if (ModelState.IsValid)
            {

                if (newUser.password == newUser.RepeatPassword)
                {
                    Users newValidUser = new Users()
                    {
                        Name = newUser.Name,
                        Username = newUser.Username,
                        PersonNum = int.Parse(newUser.Personnum),
                        Role = (Models.Roles)Enum.Parse(typeof(Models.Roles), "3"),
                        Gender = newUser.Gender,
                        //(Models.Genders)Enum.Parse(typeof(Models.Genders), newUser.Gender),
                        Email = newUser.Email,
                        Password = newUser.password,

                    };
                    if (pic != null && pic.ContentType.ToLower() == "image/jpeg")
                    {

                        BinaryReader br = new BinaryReader(pic.InputStream);
                        newValidUser.Pic = br.ReadBytes(pic.ContentLength);

                    }
                    else
                    {
                        ModelState.AddModelError("userImage", "Please post a valid jpg image");
                    }
                    CarRepository repo = new CarRepository();
                    repo.AddUser(newValidUser);
                    ViewBag.succeeded = true;
                    return View("index");

                }
                else
                {
                    ModelState.AddModelError("confirmPassword", "password don't match original password");
                }
            }
            else
            {
                var errors = ModelState
                    .Where(x => x.Value.Errors.Count > 0)
                    .Select(x => new { x.Key, x.Value.Errors })
                    .ToArray();
            }
            return View(newUser);
        }
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(string Username, string Password)
        {
            CarRepository repo = new CarRepository();
            if (repo.logValidation(Username, Password))
            {
                FormsAuthentication.SetAuthCookie(Username, true);

                
                //return Redirect(returnUrl ?? FormsAuthentication.DefaultUrl);
                return Redirect("~/"+repo.GetUser(Username).Role.ToString()+"/Index");
            }

            return View();
        }

    }
}
