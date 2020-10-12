using CA1.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CA1.Controllers;

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
            AddProducts("SeedData/product.data");
        }
        public static string GetStringSha256Hash(string text)
        {
            if (String.IsNullOrEmpty(text))
                return String.Empty;

            using (var sha = new System.Security.Cryptography.SHA256Managed())
            {
                byte[] textData = System.Text.Encoding.UTF8.GetBytes(text);
                byte[] hash = sha.ComputeHash(textData);
                return BitConverter.ToString(hash).Replace("-", String.Empty);
            }
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
                    Password = GetStringSha256Hash(usernames[i])
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
