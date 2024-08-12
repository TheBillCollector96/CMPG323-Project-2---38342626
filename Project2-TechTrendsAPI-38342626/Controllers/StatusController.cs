using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Project2_TechTrendsAPI_38342626.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatusController : ControllerBase
    {
        [HttpGet(Name = "Status")]
        public ActionResult Get()
        {
            // Returning anonymous object. You can return anything you want.
            var response = new
            {
                Message = "All good!",
                ServerTime = DateTime.Now
            };

            return Ok(response);
        }

        [HttpGet("db", Name = "DbStatus")]
        public ActionResult DbStatus()
        {
            // Here you would make a call to the database: "select getdate()"
            var date = DateTime.Now; 

            // If DB call is successful:
            var response = new
            {
                Message = "DB is up!",
                ServerTime = DateTime.Now,
                DatabaseTime = date
            };

            return Ok(response);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("auth", Name = "AuthStatus")]
        public ActionResult AuthStatus()
        {
            var username = HttpContext.User.Claims
                .FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?
                .Value;
            // If it even gets to here it means auth is working because of the [Authorize] tag
            var response = new
            {
                Message = "Auth is up!",
                ServerTime = DateTime.Now,
                Username = username
            };

            return Ok(response);
        }
    }
}
