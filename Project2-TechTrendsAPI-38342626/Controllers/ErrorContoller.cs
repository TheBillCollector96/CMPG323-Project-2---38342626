using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Project2_TechTrendsAPI_38342626.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ErrorContoller : ControllerBase
    {
        [HttpGet("401", Name = "Unauthorized")]
        public IActionResult UnAuthorized401()
        {
            return Unauthorized();
        }

        [HttpGet("403", Name = "Forbidden")]
        public IActionResult Forbidden403()
        {
            return Forbid();
        }

        [HttpGet("404", Name = "NotFound")]
        public IActionResult NotFound404()
        {
            return StatusCode(404);
        }

        [HttpGet("500", Name = "InternalServerError")]
        public IActionResult InternalServerError500()
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }
}
