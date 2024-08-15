using API.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Json;
using Microsoft.Extensions.Hosting;
using System.Diagnostics;

namespace API.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HttpClient _httpClient { get; private set; }

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
            _httpClient = new HttpClient();

        }
        public async Task<List<User>> GetFromAPIAsync()
        {
            var response = await _httpClient.GetAsync("https://dummyjson.com/users");
            response.EnsureSuccessStatusCode();
            var posts = await response.Content.ReadFromJsonAsync<List<User>>();
            return posts;
        }

        public async Task<IActionResult> GetUsers()
        {
            return View(await Task.Run(GetFromAPIAsync));
        }
        public async Task<User> PostUserAsync(User newUser)
        {
            var response = await _httpClient.PostAsJsonAsync("https://dummyjson.com/users/add", newUser);
            response.EnsureSuccessStatusCode();
            var createdPost = await response.Content.ReadFromJsonAsync<User>();
            return createdPost;
        }

        public async Task<IActionResult> CreateUser(User newUser)
        {
            var res = Task.Run(GetFromAPIAsync);

            return View(newUser);
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
