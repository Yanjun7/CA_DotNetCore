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
                    Password = usernames[i]
                });
            }
            db.SaveChanges();
        }

        public void AddProducts(string filename)
        {
            string[] lines = File.ReadAllLines(filename);
            foreach(string line in lines)
            {

            }
        }
    }
}
