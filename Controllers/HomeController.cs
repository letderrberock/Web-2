using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookStore.DataAccess;
using BookStore.Entities;
using BookStore.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.Controllers
{
    public class HomeController : Controller
    {
        private readonly PartDbContext _DataBase;

        public HomeController(PartDbContext dbContext)
        {
            _DataBase = dbContext;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Login()
        {
            if (this.HttpContext.Session.GetString("loggedUser") != null)
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginVM Login)
        {
            if (this.HttpContext.Session.GetString("loggedUser") != null)
            {
                return RedirectToAction("Index", "Home");
            }

            if (!ModelState.IsValid)
            {
                return View(Login);
            }
            var userResult = _DataBase.Users.FirstOrDefault(u => u.Username == Login.Username && u.Password == Login.Password);
            if (userResult == null)
            {
                ModelState.AddModelError("error", "Wrong username or password.");
                return View(Login);
            }
            else
            {
                HttpContext.Session.SetString("loggedUser", Login.Username);
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpGet]
        public IActionResult Logout()
        {
            if (this.HttpContext.Session.GetString("loggedUser") != null)
            {
                HttpContext.Session.Remove("loggedUser");
                return RedirectToAction("Index", "Home");
            }

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult Register()
        {
            if (this.HttpContext.Session.GetString("loggedUser") != null)
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        [HttpPost]
        public IActionResult Register(RegisterVM newUser)
        {
            if (this.HttpContext.Session.GetString("loggedUser") != null)
            {
                return RedirectToAction("Index", "Home");
            }

            if (ModelState.IsValid)
            {
                Users u = new Users
                {
                    Username = newUser.Username,
                    Password = newUser.Password,
                    FirstName = newUser.FirstName,
                    LastName = newUser.LastName
                };

                _DataBase.Users.Add(u);
                _DataBase.SaveChanges();
                return RedirectToAction("Index", "Home");
            }

            return View(newUser);
        }

       
    }
}
