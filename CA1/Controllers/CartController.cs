using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using CA1.Database;
using CA1.Models;
using Microsoft.AspNetCore.Mvc;

namespace CA1.Controllers
{
    public class CartController : Controller
    {
        private readonly DbGallery db;
        public CartController(DbGallery db)
        {
            this.db = db;
        }


        //input  productId
        [HttpPost]
        public IActionResult Add([FromBody] ProductObj productObj)
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
            else
            {
                Session session = db.Sessions.FirstOrDefault(x => x.Id == sessionId);
                //string userId = session.UserId;
                ShoppingCartDetail cart = new ShoppingCartDetail()
                {
                    Id = Guid.NewGuid().ToString(),
                    UserId = session.UserId,
                    ProductId = productObj.ProductId
                };

                db.ShoppingCart.Add(cart);
                db.SaveChanges();

                return Json(new
                {
                    status = "success"
                });
            }
            
        }
    }
}
