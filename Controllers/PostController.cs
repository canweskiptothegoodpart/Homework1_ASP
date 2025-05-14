using Homework1_ASP.Models;
using Microsoft.AspNetCore.Mvc;

namespace Homework1_ASP.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PostController: ControllerBase
    {
        private static List<Post> Posts = new List<Post>
        {
            new Post { UserId = 1, Id = 1, Title = "First Post", Body = "This is the first post" },
            new Post { UserId = 2, Id = 2, Title = "Second Post", Body = "This is the second post" }
        };


        [HttpGet("{userId}, {title}")]
        public ActionResult<List<Post>> GetPost(int userId, string title)
        {
            var posts = Posts.Where(p => p.UserId == userId && p.Title == title).ToList();
            if (!posts.Any()) return NoContent();
            return Ok(posts);
        }

        [HttpDelete("{id}")]
        public ActionResult DeletePost(int id)
        {
            var post = Posts.FirstOrDefault(p => p.Id == id);
            if (post != null)
            {
                Posts.Remove(post);
            }
            return NoContent();
        }
    }
}
