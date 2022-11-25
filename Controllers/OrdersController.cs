using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookStore.ActionFilter;
using BookStore.DataAccess;
using BookStore.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.Controllers
{
    [AuthenticationFilter]
    public class OrdersController : Controller
    {
       
            private readonly PartDbContext _DataBase;

            public OrdersController(PartDbContext dbContext)
            {
                _DataBase = dbContext;
            }
            public IActionResult Index()
            {

                var allorders = _DataBase.Orders.ToList();
                List<OrderItems> oiList = new List<OrderItems>();
                foreach (var item in allorders)
                {

                    var part = _DataBase.Books.FirstOrDefault(b => item.BookId == b.Id);
                    var username = HttpContext.Session.GetString("loggedUser");
                    OrderItems oi = new OrderItems
                    {
                        BookId = part.Id,
                        BookName = part.Name,
                        UserName = username
                    };
                    oiList.Add(oi);

                }

                OrdersList pushList = new OrdersList
                {
                    OrderList = oiList
                };
                    return View(pushList);

            }

        [HttpGet]
        public IActionResult Delete(int id)
        {

            var order = _DataBase.Orders.FirstOrDefault(o => o.BookId == id);
            if (order == null)
            {
                return NotFound();
            }

            _DataBase.Orders.Remove(order);
            _DataBase.SaveChanges();
            return RedirectToAction("Index","Orders");
        }
    }
}
