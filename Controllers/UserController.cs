using Homework1_ASP.Models;
using Microsoft.AspNetCore.Mvc;

namespace Homework1_ASP.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
       
        private readonly HttpClient _httpClient;
        private readonly string _jsonPlaceholderBaseUrl;
        private readonly string _reqResBaseUrl;

        public UserController(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClient = httpClientFactory.CreateClient();
            _jsonPlaceholderBaseUrl = configuration["ApiUrls:JsonPlaceholder"];
            _reqResBaseUrl = configuration["ApiUrls:ReqRes"];
        }


        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] User newUser)
        {
            var url = $"{_reqResBaseUrl}/users";
            var response = await _httpClient.PostAsJsonAsync(url, newUser);
            if (!response.IsSuccessStatusCode)
                return StatusCode((int)response.StatusCode, await response.Content.ReadAsStringAsync());

            var createdUser = await response.Content.ReadFromJsonAsync<User>();
            return Created("users/" + createdUser.Id, createdUser);
        }

        //[HttpGet("{id}")]
        //public ActionResult<User> GetUserById(int id)
        //{
        //    var user = Users.FirstOrDefault(u => u.Id == id);
        //    if (user == null) return NoContent();
        //    return Ok(user);
        //}

        [HttpPut("users/{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] User updatedUser)
        {
            var url = $"{_reqResBaseUrl}/users/{id}";
            var response = await _httpClient.PutAsJsonAsync(url, updatedUser);
            if (!response.IsSuccessStatusCode)
                return StatusCode((int)response.StatusCode, await response.Content.ReadAsStringAsync());

            var user = await response.Content.ReadFromJsonAsync<User>();
            return Ok(user);
        }
    }
}
