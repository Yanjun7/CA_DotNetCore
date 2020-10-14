using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using CA1.Database;
using CA1.Models;
using Microsoft.AspNetCore.Mvc;

namespace CA1.Controllers
{
    public class HistoryController : Controller
    {
        private readonly DbGallery db;
        public HistoryController(DbGallery db)
        {
            this.db = db;
        }
        public IActionResult Index()
        {
            string sessionId = HttpContext.Request.Cookies["sessionId"];
            Session session = db.Sessions.FirstOrDefault(x => x.Id.ToString() == sessionId);

            List<Order> orders = db.Orders.Where(x => x.UserId == session.UserId).ToList();
            ViewData["Order"] = orders;  // ==> timestamp, userId
                                         //List<string> productIds = new List<string>();
                                         //List<string> ActivationCodes = new List<string>();  = (List<OrderDetail>)db.OrderDetails.Where(x => x.OrderId == orders[i].Id);

            /*List<List<OrderDetail>> orderDetails = new List<List<OrderDetail>>();

            for (int i = 0; i < orders.Count; i++)
            {
                List<OrderDetail> temp = db.OrderDetails.Where(x => x.OrderId == orders[i].Id).ToList();
                orderDetails.Add(temp);
            }
                
            ViewData["OrderDetails"] = orderDetails; //this is a list in a list*/

            if (orders.Count == 0)
            {
                ViewData["IsLogin"] = session.IsLogin;
                ViewData["username"] = session.User.Username.ToUpper();
                return View("noOrders");
            }

            List<OrderDetail> orderDetails = db.OrderDetails.Where(x => x.Order.UserId == session.UserId).ToList();
            ViewData["OrderDetails"] = orderDetails;
            List<Product> products = db.Products.ToList();
            ViewData["Products"] = products;

            ViewData["IsLogin"] = session.IsLogin;
            ViewData["username"] = session.User.Username.ToUpper();

            return View();
        }

        [HttpPost]
        public IActionResult CheckOut()
        {
            string sessionId = HttpContext.Request.Cookies["sessionId"];
            Session session = db.Sessions.FirstOrDefault(x => x.Id.ToString() == sessionId);
            User user = db.Users.FirstOrDefault(x => x.Id == db.Sessions.FirstOrDefault(y => y.Id.ToString() == sessionId).UserId);
            
            if (user == null)
            {
                session.IsReadyToCheckOut = true;
                db.SaveChanges();

                return Json(new
                {
                    status = "notLogin",
                    url = "/Login/CheckOutUnsuccessful"
                });
            }
            else
            {
                List<ShoppingCartDetail> shoppingcart = db.ShoppingCart.Where(c => c.UserId == user.Id).ToList();

                bool quantityChecker = true;

                for (int i = 0; i < shoppingcart.Count; i++)
                {
                    if (shoppingcart[i].Quantity == 0)
                    {
                        quantityChecker = false;
                        break;
                    }
                }

                if (quantityChecker == false)
                {
                    return Json(new
                    {
                        status = "unsuccessful"
                    });
                }
                else
                {
                    Order newOrder = new Order()
                    {
                        Id = Guid.NewGuid().ToString(),
                        UserId = user.Id,
                        TransactionDate = DateTime.Now
                    };

                    db.Orders.Add(newOrder);

                    for (int i = 0; i < shoppingcart.Count; i++)
                    {
                        for (int j = 0; j < shoppingcart[i].Quantity; j++)
                        {
                            db.OrderDetails.Add(new OrderDetail
                            {
                                Id = Guid.NewGuid().ToString(),
                                OrderId = newOrder.Id,
                                ProductId = shoppingcart[i].ProductId,
                                ActivationCode = Guid.NewGuid()
                            });
                        }
                        db.ShoppingCart.Remove(shoppingcart[i]);
                    }

                    db.SaveChanges();

                    return Json(new
                    {
                        status = "success",
                        url = "/History/Index"
                    });
                }
            }
        }

        public IActionResult Unsuccessful()
        {
            return View();
        }
    }
}