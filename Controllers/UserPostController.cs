using Homework1_ASP.Models;
using Microsoft.AspNetCore.Mvc;

namespace Homework1_ASP.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserPostController : ControllerBase
    {
        private static List<Post> Posts = new List<Post>
        {
            new Post { UserId = 1, Id = 1, Title = "First Post", Body = "This is the first post" },
            new Post { UserId = 2, Id = 2, Title = "Second Post", Body = "This is the second post" }
        };

        private static List<User> Users = new List<User>
        {
            new User { Id = 1, Email = "user1@example.com", FirstName = "John", LastName = "Doe", Avatar = "avatar1.png" },
            new User { Id = 2, Email = "user2@example.com", FirstName = "Jane", LastName = "Smith", Avatar = "avatar2.png" }
        };

        [HttpGet("search")]
        public IActionResult GetPost(int userId, string title)
        {
            var post = Posts.FirstOrDefault(p => p.UserId == userId && p.Title == title);
            if (post == null) return NotFound("Post not found");
            return Ok(post);
        }

        [HttpGet]
        public IActionResult GetAllPosts()
        {
            return Ok(Posts);
        }

        [HttpPost("users")]
        public IActionResult CreateUser([FromBody] User newUser)
        {
            if (Users.Any(u => u.Id == newUser.Id))
                return BadRequest("User with this ID already exists");

            Users.Add(newUser);
            return CreatedAtAction(nameof(CreateUser), newUser);
        }

        [HttpPut("users/{id}")]
        public IActionResult UpdateUser(int id, [FromBody] User updatedUser)
        {
            var user = Users.FirstOrDefault(u => u.Id == id);
            if (user == null) return NotFound("User not found");

            user.Email = updatedUser.Email;
            user.FirstName = updatedUser.FirstName;
            user.LastName = updatedUser.LastName;
            user.Avatar = updatedUser.Avatar;

            return Ok(user);
        }

        [HttpDelete("{id}")]
        public IActionResult DeletePost(int id)
        {
            var post = Posts.FirstOrDefault(p => p.Id == id);
            if (post == null) return NotFound("Post not found");

            Posts.Remove(post);
            return NoContent();
        }
    }
}
