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
        [HttpPost("CreateUser")]
        public async Task<IActionResult> CreateUser([FromBody] UserRequestModel model)
        {
            if (model == null)
            {
                return BadRequest("Invalid request data");
            }

            // Serialize your model to JSON and create a StringContent with it
            var content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");

            // Send the POST request to your User endpoint
            HttpResponseMessage response = await _httpClient.PostAsync("User", content);

            if (response.IsSuccessStatusCode)
            {
                // Read the response content as a string
                string responseContent = await response.Content.ReadAsStringAsync();

                // Log the response to the console
                Console.WriteLine($"User created successfully! Response: {responseContent}");

                // Return a success response
                return Ok(responseContent);
            }
            else
            {
                // Log the error status code
                Console.WriteLine($"Error: {response.StatusCode}");

                // Return an error response
                return StatusCode((int)response.StatusCode, "Error occurred during user creation.");
            }
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
    public string? FirstName { get; set; }
    [Required]
    public string? LastName { get; set; }
}