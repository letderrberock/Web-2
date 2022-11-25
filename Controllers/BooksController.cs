using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookStore.DataAccess;
using Microsoft.AspNetCore.Mvc;
using BookStore.ViewModels;
using Microsoft.AspNetCore.Http;
using BookStore.Entities;
using BookStore.ActionFilter;

namespace BookStore.Controllers
{
    [AuthenticationFilter]
    public class BooksController : Controller
    {
        private readonly PartDbContext _DataBase;

        public BooksController(PartDbContext dbContext)
        {
            _DataBase = dbContext;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var getBooks = _DataBase.Books.ToList();
            BookList pList = new BookList
            {
                bookList = getBooks
            };
            return View(pList);
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            var part = _DataBase.Books.FirstOrDefault(b => b.Id == id);
            if (part == null) {
                return NotFound();
            }

            _DataBase.Remove(part);
            _DataBase.SaveChanges();
            return RedirectToAction("Index", "Books");
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {

            var username = HttpContext.Session.GetString("loggedUser");
            var thisUser = _DataBase.Users.FirstOrDefault(u => u.Username == username);
            var Part = _DataBase.Books.FirstOrDefault(c => c.Id == id);
            if (thisUser == null || Part == null)
            {
                return NotFound();
            }

            BookVM partDataToSend = new BookVM
            {
                Id = Part.Id,
                Name = Part.Name,
                Description = Part.Description,
                UserName = thisUser.FirstName
            };
            return View(partDataToSend);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(BookVM updatedPart)
        {

            var part = _DataBase.Books.FirstOrDefault(c => c.Id == updatedPart.Id);
            if (part == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                part.Name = updatedPart.Name;
                part.Description = updatedPart.Description;

                _DataBase.Books.Update(part);
                _DataBase.SaveChanges();

                return RedirectToAction("Index", "Books");
            }

            return View(updatedPart);
        }

        [HttpGet]
        public IActionResult Create()
        {

            return View();
        }

        [HttpPost]
        public IActionResult Create(CreateBook newPart)
        {

            if (ModelState.IsValid) {
                string userName = HttpContext.Session.GetString("loggedUser");
                Users user = _DataBase.Users.FirstOrDefault(u => u.Username == userName);

                Books nPart = new Books
                {
                    Name = newPart.Name,
                    Description = newPart.Description,
                    UserId = user.Id
                };

                _DataBase.Books.Add(nPart);
                _DataBase.SaveChanges();
                return RedirectToAction("Index", "Books");
            }

           

            return View(newPart);
        }

        [HttpGet]
        public IActionResult Add(int BookId)
        {

            var dublicate = _DataBase.Orders.FirstOrDefault(o => o.BookId == BookId);
            if (dublicate != null)
            {
                return RedirectToAction("Index", "Books");
            }
           

            string userName = HttpContext.Session.GetString("loggedUser");
            Users user = _DataBase.Users.FirstOrDefault(u => u.Username == userName);
            Orders b = new Orders
            {
                BookId = BookId,
                UserId = user.Id
            };

            _DataBase.Orders.Add(b);
            _DataBase.SaveChanges();
            return RedirectToAction("Index","Books");
        }

        

    }
}
