using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CA1.Database;
using CA1.Models;
using Microsoft.AspNetCore.Mvc;

namespace CA1.Controllers
{
    public class PaymentController : Controller
    {
        private readonly DbGallery db;
        public PaymentController(DbGallery db)
        {
            this.db = db;
        }
        public IActionResult Index()
        {
            string sessionId = HttpContext.Request.Cookies["sessionId"];
            if (sessionId == null)
            {
                return Json(new
                {
                    status = "redirect",
                    url = "/Login/Index"
                });
            }
            Session session = db.Sessions.FirstOrDefault(x => x.Id == sessionId);
            List<Order> orders = (List<Order>) db.Orders.Where(x => x.UserId == session.UserId);
            ViewData["Order"] = orders;  // ==> timestamp, userId
            //List<string> productIds = new List<string>();
            //List<string> ActivationCodes = new List<string>();  = (List<OrderDetail>)db.OrderDetails.Where(x => x.OrderId == orders[i].Id);
            List<OrderDetail> orderDetails = new List<OrderDetail>();
            for (int i = 0; i < orders.Count(); i++)
            {
                orderDetails.Add((OrderDetail) orders[i].OrderDetails);
            }
            ViewData["OrderDetails"] = orderDetails;

            return View();
        }
    }
}
