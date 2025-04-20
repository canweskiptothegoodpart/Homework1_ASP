using Homework1_ASP.Models;
using Microsoft.AspNetCore.Mvc;

namespace Homework1_ASP.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PostController: ControllerBase
    {
    
        private readonly HttpClient _httpClient;
        private readonly string _jsonPlaceholderBaseUrl;
        private readonly string _reqResBaseUrl;

        public PostController(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClient = httpClientFactory.CreateClient();
            _jsonPlaceholderBaseUrl = configuration["ApiUrls:JsonPlaceholder"];
            _reqResBaseUrl = configuration["ApiUrls:ReqRes"];
        }


        [HttpGet]
        public async Task<IActionResult> GetPost([FromQuery] int? userId, [FromQuery] string title)
        {
            var query = new List<string>();
            if (userId.HasValue) query.Add($"userId={userId.Value}");
            if (!string.IsNullOrEmpty(title)) query.Add($"title={System.Net.WebUtility.UrlEncode(title)}");

            var queryString = query.Any() ? "?" + string.Join("&", query) : string.Empty;
            var url = $"{_jsonPlaceholderBaseUrl}/posts{queryString}";

            var posts = await _httpClient.GetFromJsonAsync<List<Post>>(url);
            if (posts == null || !posts.Any()) return NoContent();
            return Ok(posts);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePost(int id)
        {
            var url = $"{_jsonPlaceholderBaseUrl}/posts/{id}";
            var response = await _httpClient.DeleteAsync(url);
            return NoContent(); 
        }
    }
}
