using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Http;

namespace Project2_TechTrendsAPI_38342626.Authorisation
{
    public class BasicAuthenticationHandler 
    {
        private readonly RequestDelegate next;
        private readonly string relm;

        public BasicAuthenticationHandler(RequestDelegate next, string relm)
        {
            this.next = next;
            this.relm = relm;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (!context.Request.Headers.ContainsKey("Authorization"))
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("Unauthorized");
                return;
            }

            //Basic userId:password
            var header = context.Request.Headers["Authorization"].ToString();
            var encodedCreds = header.Substring(6);
            var creds = Encoding.UTF8.GetString(Convert.FromBase64String(encodedCreds));

            string[] uidpwd = creds.Split(':');
            var uid = uidpwd[0];
            var password = uidpwd[1];

            if(uid != "NWUTechTrendsAPI" || password != "password")
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("Unauthorized");
                return;
            }

            await next(context);

        }
    }
}
