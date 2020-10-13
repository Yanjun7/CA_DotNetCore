using CA1.Controllers;
using CA1.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CA1.Database
{
    public class DbSeedData
    {
        private readonly DbGallery db;

        public DbSeedData(DbGallery db)
        {
            this.db = db;
        }

        public void Init()
        {
            AddUsers();
            AddProducts("SeedData/productGame.data");
        }

        public void AddUsers()
        {
            string[] usernames = { "john", "mary" };

            for (int i = 0; i < usernames.Length; i++)
            {
                db.Users.Add(new User
                {
                    Id = "user_" + (1000 + (i+1)),
                    Username = usernames[i],
                    Password = LoginController.GetStringSha256Hash(usernames[i])
                });
            }
            db.SaveChanges();
        }

        public void AddProducts(string filename)
        {
            string[] lines = File.ReadAllLines(filename);
            foreach(string line in lines)
            {
                string[] pair = line.Split(";");
                if (pair.Length == 5)
                {
                    db.Products.Add(new Product
                    {
                        Id = Guid.NewGuid().ToString(),
                        PhotoLink = pair[0],
                        ProductName = pair[1],
                        Price = Convert.ToDouble(pair[2]),
                        Description = pair[3],
                        PhotoTag = pair[4]
                    });
                }
            }
            db.SaveChanges();
        }
    }
}
