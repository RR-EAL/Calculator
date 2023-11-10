using System.ComponentModel.DataAnnotations;
using System.Net.Http;
using System.Text;
using System.Xml.Schema;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.VisualBasic.Syntax;
using Newtonsoft.Json;
using TodoApi.Models;


namespace TodoApi.Controllers
{
    [Route("Traka")]
    [ApiController]
    public class TrakaController : ControllerBase
    {
        public static List<string> authorisations = new List<string>
        {
            "X79",
            "Q55"
        };

        [HttpPost("User")]

        public UserResponseModel? CreateUser([FromBody] UserRequestModel model)
        {
            if (model == null)
            {
                return null;
            }
            Console.WriteLine("User created successfully!");
            authorisations.Add(model.ForeignKey);
            // Return a success response
            return new UserResponseModel
            {
                ForeignKey = model.ForeignKey,
                PrimaryKey = "Primary_Key",
                Forename = model.Forename,
                Surname = model.Surname,
            };
        }



        [HttpDelete("User/foreignKey/{userKey}")]

        public IActionResult DeleteUser(string userKey, [FromBody] PermissionsSetModel model)
        {
            string auth = null;
            foreach (var permission in authorisations)
            {
                if (permission == userKey)
                {
                    auth = permission;
                }
            }

            if (auth == null)
            {
                return NotFound();
            }

            authorisations.Remove(auth);
            //opslaan model
            return Ok();
        }




        [HttpPost("User/foreignKey/{userKey}/ItemAccess")]

        public IActionResult AssignNewSetOfPermissionsForSpecifiecUser(string userKey, [FromBody] PermissionsSetModel model)
        {
            string auth = null;
            foreach (var permission in authorisations)
            {
                if (permission == userKey)
                {
                    auth = permission;
                }
            }

            if (auth == null)
            {
                return NotFound();
            }

            //opslaan model
            return Ok();

        }



        private string Version = "2.7.12.0";

        [HttpGet("Version")] 
        public IActionResult GetVersion()
        {
 

            return Ok(Version);
        }



        [HttpHead("User/foreignKey/{userKey}")]

        public IActionResult CheckIfAUserExists(string userKey)
        {
            foreach (var item in authorisations)
            {
                if (userKey == item)
                {
                    Console.WriteLine("user exits");
                    return Ok();
                }
            }
            Console.WriteLine($"user {userKey} not found");
            return NotFound();


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


        [HttpPut("User/foreignKey/{userKey}")]
        public IActionResult UpdateUser(string userKey, UserRequestModel model)
        {
            foreach (var item in authorisations)
            {
                if (userKey == item)
                {
                    Console.WriteLine("user exits");
                    return Ok();
                }
            }
            Console.WriteLine($"user {userKey} not found");
            return NotFound();


        }




        internal List<string> FindAllAuthorisationsInternal()
        {
            Console.WriteLine(authorisations);
            return authorisations;
        }
    }
}







