using System.ComponentModel.DataAnnotations;
using System.Net.Http;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace TodoApi.Controllers
{
    [Route("Traka")]
    [ApiController]
    public class TrakaController : ControllerBase
    {

        [HttpPost("User")]

        public IActionResult CreateUser([FromBody] UserRequestModel model)
        {
            if (model == null)
            {
                return BadRequest("Invalid request data");
            }
            Console.WriteLine("User created successfully!");

            // Return a success response
            return Ok($"{model.Surname} created successfully!");
        }

        [HttpGet("Version")]
        public string GetVersion()
        {
            return "<string>2.20.4.0</string>";
        }
    }
}



public class UserRequestModel
{
    [Required]
    public string? ForeignKey { get; set; }
    [Required]
    public string? Forename { get; set; }
    [Required]
    public string? Surname {  get; set; }
    public int? CardId { get; set; }
    public int? Pin {  get; set; }
    public DateTime? PinExpiryDate { get; set; }
    public Boolean? ActiveFlag { get; set; }
    public DateTime? ActiveDate { get; set;} 
    public DateTime? ExpiryDate { get; set; }
    public string? Detail {  get; set; }


}