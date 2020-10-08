using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using CA1.Models;
using CA1.Database;
using Microsoft.VisualBasic;

namespace CA1.Controllers
{
    public class ShopCartController : Controller
    {
        private readonly DbGallery db;

        public ShopCartController(DbGallery db)
        {
            this.db = db;
        }
        public IActionResult ShopCart()
        {
            List<ShoppingCartDetail> carts = db.ShoppingCart.ToList();
            if (carts.Count == 0)
            {
                bool count = true;
                ViewData["count"] = count;
                return View();
            }
            else
            {
                bool count = true;
                ViewData["count"] = count;

                string[] productid = new string[carts.Count];
            int[] quantity = new int[carts.Count];
            for (int i = 0; i < carts.Count; i++)
            {
                productid[i] = carts[i].ProductId;
                quantity[i] = carts[i].Quantity;
            }

            List<Product> products = db.Products.ToList();
            string[] ids = new string[products.Count];
            string[] images = new string[products.Count];
            string[] names = new string[products.Count];
            double[] prices = new double[products.Count];
            string[] info = new string[products.Count];
            for (int i = 0; i < products.Count; i++)
            {
                ids[i]= products[i].Id;
                images[i] = products[i].PhotoLink;
                names[i] = products[i].ProductName;
                prices[i] = products[i].Price;
                info[i] = products[i].Description;
            }
            int[] keyid = new int[carts.Count];
            for(int i=0;i<carts.Count;i++)
            {
                for(int j=0;j<products.Count;j++)
                {
                    if (productid[i] == ids[j])
                        keyid[i] = j;
                }
            }
            string[] Names = new string[keyid.Length];
            string[] Images = new string[keyid.Length];
            double[] Prices = new double[keyid.Length];
            string[] Info = new string[keyid.Length];
            double total=0;
            for (int i=0;i<keyid.Length;i++)
            {
                Names[i] = names[keyid[i]];
                Images[i] = images[keyid[i]];
                Prices[i] = prices[keyid[i]]*quantity[i];
                Info[i] = info[keyid[i]];
                    total = total + Prices[i];
            }

               
                

                ViewData["images"] = Images;
                ViewData["names"] = Names;
                ViewData["informations"] = Info;
                ViewData["prices"] = Prices;
                ViewData["quantity"] = quantity;
                ViewData["total"] = total;
                 

                //测试用！
                //ViewData["images"] = images;
                //ViewData["names"] = names;
                //ViewData["informations"] = info;
                //ViewData["prices"] = prices;
                //ViewData["quantity"] = quantity;

                return View();
            }
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}