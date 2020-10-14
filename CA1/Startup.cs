using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using CA1.Database;
using CA1.Controllers;
using CA1.Models;
using CA1.Middlewares;

namespace CA1
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();            

            //adding db context into DI container
            services.AddDbContext<DbGallery>(opt =>
                opt.UseLazyLoadingProxies().UseSqlServer(
                    Configuration.GetConnectionString("DbConn")
                )
            );
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, DbGallery db)
        {
            app.UseDeveloperExceptionPage();
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            //app.UseMiddleware<SessionChecker>();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });

            //!!!WARNING!!!: This is used to delete the DB if you want a clean slate
            db.Database.EnsureDeleted();

            //this makes sure that the Db is create and available on your machine/server
            db.Database.EnsureCreated();

            //this is to clean off all the ShoppingCart record that is added by Guest
            List<ShoppingCartDetail> GuestRecord = db.ShoppingCart.Where(x => x.UserId == null).ToList();
            foreach (ShoppingCartDetail item in GuestRecord)
            {
                db.ShoppingCart.Remove(item);
            }
            db.SaveChanges();

            //this is too seed data when DB is newly created and empty
            new DbSeedData(db).Init();
        }
    }
}
