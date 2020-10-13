using CA1.Database;
using Microsoft.AspNetCore.Http;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using CA1.Models;

namespace CA1.Middlewares
{
    public class SessionChecker
    {
        private readonly RequestDelegate next;



        public SessionChecker(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task Invoke(HttpContext context, DbGallery db)
        {
            string sessionId = context.Request.Cookies["sessionId"];
            Session session = db.Sessions.FirstOrDefault(x => x.Id.ToString() == sessionId);

            if (session != null && session.User != null)
            {
                Session currentSession = db.Sessions.FirstOrDefault(x => x.Id == Guid.Parse(sessionId));
                long oldTimestamp = currentSession.Timestamp;
                long newTimestamp = DateTimeOffset.Now.ToUnixTimeSeconds();
                if (newTimestamp - oldTimestamp > 5) // change it to 1200!!!!!!!!!!!!!!!!!!!!!!!
                {
                    db.Sessions.Remove(currentSession);
                    db.SaveChanges();
                    context.Response.Cookies.Delete("sessionId");
                    context.Response.Redirect("/Login/Index");
                    return;
                }
                else
                {
                    currentSession.Timestamp = newTimestamp;
                    db.SaveChanges();
                }
            }
            else if (session != null)
            {
                session.Timestamp = DateTimeOffset.Now.ToUnixTimeSeconds();
                db.SaveChanges();
            }

            await next(context);
        }
    }
}