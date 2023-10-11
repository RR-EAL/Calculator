using System.ComponentModel.DataAnnotations;
using System.Net.Http;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using TodoApi.Models;

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
            "Authorisation5",
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

        /// <summary>
        /// Fake implementatie van 3.7.1.1 RETURN A PAGED LIST OF USERS
        /// </summary>
        /// <returns></returns>
        [HttpGet("User/page/{page}/pageSize/{pageSize}")]
        public IActionResult FindAllUsers(int page, int pageSize)
        {
            try
            {
                // Call the internal method to get the list of authorisations
                var authorisationsList = FindAllAuthorisationsInternal();
                var skip = pageSize * page;

                List<UserResponseModel> items = new();
                foreach (string item in authorisationsList.Skip(page - 1))
                {
                    if (item == null)
                        continue;

                    //if (skip <= item.Count())
                    //    continue;
                    
                    items.Add(new UserResponseModel
                    {
                        Surname = item,
                        ForeignKey = item,
                        PrimaryKey = item,
                        Forename = "Joe",
                    });

                    if (items.Count >= pageSize)
                        break;
                }
/*                  var its = authorisationsList
                      .Where(item => null != item)
                      .Skip(skip)
                      .Take(page)
                      .Select(item =>
                      new UserResponseModel
                      {
                          Surname = item,
                          ForeignKey = item,
                          PrimaryKey = item,
                          Forename = "Joe",
                      }
                  );*/

                return Ok(items);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred: " + ex.Message);
            }
        }

        internal List<string> FindAllAuthorisationsInternal()
        {
            Console.WriteLine(authorisations);
            return authorisations;
        }
    }
}







