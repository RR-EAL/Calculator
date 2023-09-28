using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace TodoApi.Controllers
{
    [Route("Traka")]
    [ApiController]
    public class TrakaController : ControllerBase
    {
        [HttpGet("Version")]
        public string GetVersion()
        {
            return "<string>2.20.4.0</string>";
        }

        [HttpPost("User")]
        public IActionResult CreateUser([FromBody] UserRequestModel model)
        {
            if (model == null)
            {
                return BadRequest("Invalid request data");
            }

            return Ok("User created succesfully!");
        }
    }
}



public class UserRequestModel
{
    public string? FirstName { get; set; }
    [Required]
    public string? LastName { get; set; }
}