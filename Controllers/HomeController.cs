using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using wall.Models;
using System.Linq;

namespace wall.Controllers
{
    public class HomeController : Controller
    {
        private UserContext _context;
 
        public HomeController(UserContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Route("register")]
        public IActionResult Register()
        {
            return View("Register");
        }


        [HttpGet]
        [Route("account/forgot-password")]
        public IActionResult Reset()
        {
            return View("ResetPassword");
        }

        [HttpPost]
        [Route("reset-password")]
        public IActionResult Reset_Password(string email, string password, string config_password)
        {
            User userEmail = _context.users.Where(x=>x.email == email).SingleOrDefault();
            if (userEmail != null && password == config_password){
                userEmail.password = password;
                _context.SaveChanges();
                TempData["passwordChanged"] = "Password has been successfully updated. Please login.";
                return View("Index");
                
            }
            if(userEmail == null){
                TempData["emailError"] = "The email you entered was not found in the system.";
                return View("ResetPassword");
            }
            if(password != config_password){
            TempData["passwordMatch"] = "Passwords need to match.";
            return View("ResetPassword");
            }
            return View ("ResetPassword");
        }

        [HttpPost]
        [Route("create")]
        public IActionResult Register(RegisterViewModel anything)
        {
            //first check if user is in db or not
            User user = _context.users.Where(u => u.email == anything.email).SingleOrDefault();
            if (user != null){
                ViewBag.UserExistError = "User already exist with that email. Please create a new account or login instead";
                return View("Index");
            }
            // if user is null then follow below:
            //if everything is good to go then set up the things that needs to go to the database
            if(TryValidateModel(anything)== true) {
                User newUser = new User
                {
                    first_name = anything.first_name,
                    last_name = anything.last_name,
                    email = anything.email,
                    password = anything.password,
                    created_at = DateTime.Now,
                    updated_at = DateTime.Now
                };
                _context.Add(newUser);
                _context.SaveChanges();
                HttpContext.Session.SetInt32("currUserID", newUser.userID);
                ViewBag.currUser = _context.users.SingleOrDefault(u => u.userID == HttpContext.Session.GetInt32("currUserID"));
                return RedirectToAction("Whatever", "Wall"); 

            }
            else if(TryValidateModel(anything) == false){
                ViewBag.Status = "errors";
                ViewBag.errors = ModelState.Values;
                return View ("Register");
            }
            return View("Index");
        }

        [HttpPost]
        [Route("login")]
        public IActionResult Login (string email, string password){
            User userInfo = _context.users.Where(u => u.email == email).SingleOrDefault();
            //userInfo and u.email are from  the database 
            if(userInfo != null){
                if(userInfo.password == password){
                    HttpContext.Session.SetInt32("currUserID", userInfo.userID);
                    ViewBag.currUser = HttpContext.Session.GetInt32("currUserID");
                 

                    return RedirectToAction("Whatever", "Wall");
                }
                TempData["loginError"] = "Incorrect passoword";
            }
            if(userInfo == null){
                TempData["noData"] = "This email was not found in the database please register first!";
                return RedirectToAction("Index");
            }
            return View("Index");

        }

        [HttpGet]
        [Route("logout")]
        public IActionResult Logout ()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");  
      
        }   
        
    }
}
