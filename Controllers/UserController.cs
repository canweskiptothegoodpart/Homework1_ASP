using Homework1_ASP.Dto;
using Homework1_ASP.Exceptions;
using Homework1_ASP.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;
using System.Text.RegularExpressions;

namespace Homework1_ASP.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
       
        private readonly HttpClient _httpClient;
        private readonly string _jsonPlaceholderBaseUrl;
        private readonly string _reqResBaseUrl;
        private static readonly ConcurrentDictionary<string, UserDto> _users = new();

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

        [HttpPost("userform")]
        public IActionResult SubmitUserForm([FromBody] UserDto model)
        {
            if (_users.ContainsKey(model.Username))
                throw new UserExistsException();

            if (!IsPasswordValid(model.Password, model.Username))
                throw new BusinessException("Գաղտնաբառը պետք է լինի առնվազն 6 նիշ, պարունակի մեծատառ, փոքրատառ, թիվ, հայերեն տառ, հատուկ նշան և չպարունակի օգտանունը:");

            _users[model.Username] = model;
            Logger.LogUser(model);
            return Ok(new { message = "Օգտագործողը հաջողությամբ ավելացվել է։" });
        }

        private bool IsPasswordValid(string password, string username)
        {
            if (string.IsNullOrWhiteSpace(password) || password.Length < 6) return false;
            if (password.ToLower().Contains(username.ToLower())) return false;

            bool hasUpper = password.Any(char.IsUpper);
            bool hasLower = password.Any(char.IsLower);
            bool hasDigit = password.Any(char.IsDigit);
            bool hasSymbol = password.Any(ch => !char.IsLetterOrDigit(ch));
            bool hasArmenian = Regex.IsMatch(password, "[ա-֏Ա-Ֆ]");

            return hasUpper && hasLower && hasDigit && hasSymbol && hasArmenian;
        }

        [HttpPut("userform/{username}")]
        public IActionResult UpdateField(string username, [FromBody] Dictionary<string, string> updates)
        {
            if (!_users.TryGetValue(username, out var user))
                return NotFound();

            foreach (var (key, value) in updates)
            {
                typeof(UserDto).GetProperty(key)?.SetValue(user, value);
            }

            return Ok();
        }
    }
}
