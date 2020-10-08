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
             
                string userId = session.UserId;
                string productId = productObj.ProductId;
                ShoppingCartDetail cartHistory = db.ShoppingCart.FirstOrDefault(x => x.UserId == userId &&
                    x.ProductId == productId);
                if (cartHistory == null)
                {
                    ShoppingCartDetail cart0 = new ShoppingCartDetail()
                    {
                        UserId = userId,
                        ProductId = productId,
                        Quantity = 0
                    };
                    db.ShoppingCart.Add(cart0);
                }
                else 
                {
                    int qty = cartHistory.Quantity + 1;
                    ShoppingCartDetail cart = new ShoppingCartDetail()
                    {
                        UserId = userId,
                        ProductId = productId,
                        Quantity = qty
                    };

                    
                    db.ShoppingCart.Remove(cartHistory);
                    db.ShoppingCart.Add(cart);
                }
                db.SaveChanges();

                return Json(new
                {
                    status = "success"
                });
            }
            
        }
    }
}
