using Homework1_ASP.Models;
using Microsoft.AspNetCore.Mvc;

namespace Homework1_ASP.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
       
        private static List<User> Users = new List<User>
        {
            new User { Id = 1, Email = "user1@example.com", FirstName = "John", LastName = "Doe", Avatar = "avatar1.png" },
            new User { Id = 2, Email = "user2@example.com", FirstName = "Jane", LastName = "Smith", Avatar = "avatar2.png" }
        };

       
        [HttpPost]
        public ActionResult<User> CreateUser([FromBody] User newUser)
        {
            if (Users.Any(u => u.Id == newUser.Id))
                return BadRequest("User with this ID already exists");

            Users.Add(newUser);
            return CreatedAtAction(nameof(GetUserById), new { id = newUser.Id }, newUser);
        }

        [HttpGet("{id}")]
        public ActionResult<User> GetUserById(int id)
        {
            var user = Users.FirstOrDefault(u => u.Id == id);
            if (user == null) return NoContent();
            return Ok(user);
        }

        [HttpPut("users/{id}")]
        public ActionResult<User> UpdateUser(int id, [FromBody] User updatedUser)
        {
            var user = Users.FirstOrDefault(u => u.Id == id);
            if (user == null) return NotFound("User not found");

            user.Email = updatedUser.Email;
            user.FirstName = updatedUser.FirstName;
            user.LastName = updatedUser.LastName;
            user.Avatar = updatedUser.Avatar;

            return Ok(user);
        }
    }
}
