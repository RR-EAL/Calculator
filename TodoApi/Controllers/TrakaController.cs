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
        public List<string> authorisations = new List<string>
        {
            "Authorisation1",
            "Authorisation2",
            "Authorisation3",
            "Authorisation4",
        };

        [HttpPost("User")]

        public UserResponseModel? CreateUser([FromBody] UserRequestModel model)
        {
            if (model == null)
            {
                return null;
            }
            Console.WriteLine("User created successfully!");

            // Return a success response
            return new UserResponseModel
            {
                ForeignKey = model.ForeignKey,
                PrimaryKey = "Primary_Key",
                Forename = model.Forename,
                Surname = model.Surname,
            };
        }

        [HttpGet("Version")]
        public string GetVersion()
        {
            return "<string>2.20.4.0</string>";
        }

        [HttpGet("FindAll")]
        public IActionResult FindAllAuthorisationsController()
        {
            try
            {
                // Call the internal method to get the list of authorisations
                var authorisationsList = FindAllAuthorisationsInternal();
                return Ok(authorisationsList);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred: " + ex.Message);
            }
        }

        internal List<string> FindAllAuthorisationsInternal()
        {
            return authorisations;
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

public class UserResponseModel
{
    public string? PrimaryKey { get; set; }
    [Required]
    public string? ForeignKey { get; set; }
    [Required]
    public string? Forename { get; set; }
    public string? Surname { get; set; }
}


