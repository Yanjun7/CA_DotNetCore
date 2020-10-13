using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CA1.Database;
using CA1.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CA1.Controllers
{
    public class ShoppingCartController : Controller
    {
        private readonly DbGallery db;

        public ShoppingCartController(DbGallery db)
        {
            this.db = db;
        }

        public IActionResult Index()
        {
            string sessionId = HttpContext.Request.Cookies["sessionId"];

            Session currentSession = db.Sessions.FirstOrDefault(x => x.Id.ToString() == sessionId);
            List <ShoppingCartDetail> cart = null;

            if (currentSession == null)
            {
                ViewData["count"] = false;
                ViewData["total"] = 0.00.ToString("0.00");
                ViewData["IsLogin"] = false;
                return View();
            }
            else
            {
                if (currentSession.User != null)
                {
                    cart = db.ShoppingCart.Where(x => x.UserId == currentSession.UserId).ToList();
                    ViewData["username"] = currentSession.User.Username.ToUpper();
                }
                else
                    cart = db.ShoppingCart.Where(x => x.SessionId == currentSession.Id.ToString()).ToList();

                if (cart.Count == 0)
                {
                    ViewData["count"] = false;
                }
                else
                {
                    ViewData["count"] = true;
                }

                int cartCount = cart.Count;

                string[] productId = new string[cartCount];
                string[] images = new string[cartCount];
                string[] names = new string[cartCount];
                string[] info = new string[cartCount];
                double[] prices = new double[cartCount];
                int[] quantity = new int[cartCount];
                double total = 0;

                for (int i = 0; i < cartCount; i++)
                {
                    productId[i] = cart[i].ProductId;
                    images[i] = cart[i].Product.PhotoLink;
                    names[i] = cart[i].Product.ProductName;
                    info[i] = cart[i].Product.Description;
                    prices[i] = cart[i].Product.Price;
                    quantity[i] = cart[i].Quantity;
                    total += (cart[i].Product.Price * cart[i].Quantity);
                }

                ViewData["productId"] = productId;
                ViewData["images"] = images;
                ViewData["names"] = names;
                ViewData["informations"] = info;
                ViewData["prices"] = prices;
                ViewData["quantity"] = quantity;
                ViewData["total"] = total.ToString("0.00");
                ViewData["IsLogin"] = currentSession.IsLogin;

                return View();
            }
        }

        [HttpPost]
        public IActionResult Add([FromBody] ProductObj productObj)
        {
            string sessionId = HttpContext.Request.Cookies["sessionId"];
            Session session = db.Sessions.FirstOrDefault(x => x.Id.ToString() == sessionId);
            string userId;
            string productId = productObj.ProductId;
            
            if (session == null)
            {
                Session GuestSession = new Session
                {
                    Id = Guid.NewGuid(),
                    Timestamp = DateTimeOffset.Now.ToUnixTimeSeconds(),
                    UserId = null,
                    IsLogin = false,
                    IsReadyToCheckOut = false
                };

                db.Sessions.Add(GuestSession);
                db.SaveChanges();

                Response.Cookies.Append("sessionId", GuestSession.Id.ToString());

                userId = GuestSession.UserId;
                session = GuestSession;
            }
            else
            {
                userId = session.UserId;
            }
            
            //searching for previous records in shoppingcart table
            ShoppingCartDetail cartDetail;
            if (session.User != null)
                cartDetail = db.ShoppingCart.FirstOrDefault(x => x.UserId == userId && x.ProductId == productId);
            else
                cartDetail = db.ShoppingCart.FirstOrDefault(x => x.SessionId == sessionId && x.ProductId == productId);

            if (cartDetail == null)
            {
                ShoppingCartDetail cart = new ShoppingCartDetail()
                {
                    Id = Guid.NewGuid().ToString(),
                    SessionId = session.Id.ToString(),
                    UserId = userId,
                    ProductId = productId,
                    Quantity = 1
                };

                db.ShoppingCart.Add(cart);
            }
            else
            {
                cartDetail.Quantity++;
            }

            db.SaveChanges();

            return Json(new
            {
                status = "success",
                url = "/ShoppingCart/Index"
            });
        }

        [HttpPost]
        public IActionResult Minus([FromBody] ProductObj productId)
        {
            string sessionId = HttpContext.Request.Cookies["sessionId"];
            Session session = db.Sessions.FirstOrDefault(x => x.Id.ToString() == sessionId);
            string userId = session.UserId;

            ShoppingCartDetail shoppingCartDetail;
            if (session.UserId != null)
                shoppingCartDetail = db.ShoppingCart.FirstOrDefault(x => x.UserId == userId && x.ProductId == productId.ProductId);
            else
                shoppingCartDetail = db.ShoppingCart.FirstOrDefault(x => x.SessionId == sessionId && x.ProductId == productId.ProductId);

            if (shoppingCartDetail.Quantity == 0)
                shoppingCartDetail.Quantity = 0;
            else
                shoppingCartDetail.Quantity -= 1;

            db.SaveChanges();

            return Json(new
            {
                status = "success",
                quantity = shoppingCartDetail.Quantity
            });
        }

        [HttpPost]
        public IActionResult Plus([FromBody] ProductObj productId)
        {
            string sessionId = HttpContext.Request.Cookies["sessionId"];
            Session session = db.Sessions.FirstOrDefault(x => x.Id.ToString() == sessionId);
            string userId = session.UserId;

            ShoppingCartDetail shoppingCartDetail;
            if (session.UserId != null)
                shoppingCartDetail = db.ShoppingCart.FirstOrDefault(x => x.UserId == userId && x.ProductId == productId.ProductId);
            else
                shoppingCartDetail = db.ShoppingCart.FirstOrDefault(x => x.SessionId == sessionId && x.ProductId == productId.ProductId);

            if (shoppingCartDetail.Quantity == 20)
                shoppingCartDetail.Quantity = 20; //setting a limit of maximum 20 in quantity per product, per transaction
            else
                shoppingCartDetail.Quantity += 1;

            db.SaveChanges();

            return Json(new
            {
                status = "success",
                quantity = shoppingCartDetail.Quantity
            });
        }

        [HttpPost]
        public IActionResult Remove([FromBody] ProductObj productId)
        {
            string sessionId = HttpContext.Request.Cookies["sessionId"];
            Session session = db.Sessions.FirstOrDefault(x => x.Id.ToString() == sessionId);
            
            ShoppingCartDetail shoppingCartDetail;
            if (session.User != null)
                shoppingCartDetail = db.ShoppingCart.FirstOrDefault(x => x.UserId == session.UserId && x.ProductId == productId.ProductId);
            else
                shoppingCartDetail = db.ShoppingCart.FirstOrDefault(x => x.SessionId == sessionId && x.ProductId == productId.ProductId);

            db.ShoppingCart.Remove(shoppingCartDetail);
            db.SaveChanges();

            List<ShoppingCartDetail> list = db.ShoppingCart.Where(x => x.UserId == session.UserId).ToList();
            return Json(new
            {
                status = "success",
                id = shoppingCartDetail.ProductId,
                count = list.Count,
                url = "/ShoppingCart/Index"
            });
        }

        public IActionResult Total()
        {
            string sessionId = HttpContext.Request.Cookies["sessionId"];
            Session currentSession = db.Sessions.FirstOrDefault(x => x.Id.ToString() == sessionId);

            List<ShoppingCartDetail> cart;
            
            if(currentSession.UserId != null)
                cart = db.ShoppingCart.Where(x => x.UserId == currentSession.UserId).ToList();
            else
                cart = db.ShoppingCart.Where(x => x.SessionId == sessionId).ToList();

            int cartCount = cart.Count;
            double Total = 0;

            for (int i = 0; i < cartCount; i++)
            {
                Total += cart[i].Product.Price * cart[i].Quantity;
            }

            return Json(new
            {
                status = "success",
                total = Total.ToString("0.00")
            });
        }

        public IActionResult CartIcon()
        {
            string sessionId = HttpContext.Request.Cookies["sessionId"];

            if (sessionId != null)
            {
                Session session = db.Sessions.FirstOrDefault(x => x.Id.ToString() == sessionId);
                //List<ShoppingCartDetail> cartItem = db.ShoppingCart.Where(x => x.UserId == session.UserId).ToList();

                List<ShoppingCartDetail> cartItem;

                if (session.UserId != null)
                    cartItem = db.ShoppingCart.Where(x => x.UserId == session.UserId).ToList();
                else
                    cartItem = db.ShoppingCart.Where(x => x.SessionId == sessionId).ToList();

                int cartItemCount = 0;

                foreach (ShoppingCartDetail i in cartItem)
                {
                    cartItemCount += i.Quantity;
                }

                if (cartItemCount == 0)
                {
                    return Json(new
                    {
                        Count = 0
                    });
                }
                else
                {
                    return Json(new
                    {
                        Count = cartItemCount
                    });
                }
            }
            else
            {
                return Json(new
                {
                    Count = ""
                });
            }
        }

        public IActionResult MergeCart()
        {
            string sessionId = HttpContext.Request.Cookies["sessionId"];
            Session session = db.Sessions.FirstOrDefault(x => x.Id.ToString() == sessionId);
            List<ShoppingCartDetail> GuestCart = db.ShoppingCart.Where(x => x.SessionId == sessionId).ToList();
            List<ShoppingCartDetail> UserCart = db.ShoppingCart.Where(x => x.UserId == session.UserId).ToList();

            for (int i = 0; i < GuestCart.Count; i++)
            {
                GuestCart[i].UserId = session.UserId;
                for (int j = 0; j < UserCart.Count; j++)
                {
                    if (GuestCart[i].ProductId == UserCart[j].ProductId)
                    {
                        UserCart[j].Quantity += GuestCart[i].Quantity;
                        db.ShoppingCart.Remove(GuestCart[i]);
                        db.SaveChanges();
                    }
                }
            }

            db.SaveChanges();

            if (session.IsReadyToCheckOut != true)
                return Redirect("/Home/Index");
            else
                return Redirect("/ShoppingCart/Index");
        }
    }
}
