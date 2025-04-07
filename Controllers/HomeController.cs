using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using MovieBookingMVC.Models;
using System.Diagnostics;
namespace MovieBookingMVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly HttpClient _httpClient;

        public HomeController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // Hiển thị trang home với banner
        public IActionResult Index()
        {
            return View();
        }

        // Hiển thị danh sách phim từ API
        public async Task<IActionResult> NowShowing()
        {
            var response = await _httpClient.GetAsync("https://localhost:7279/api/movies");
            if (!response.IsSuccessStatusCode)
            {
                return View(new List<Movies>());
            }

            var json = await response.Content.ReadAsStringAsync();
            var movies = JsonConvert.DeserializeObject<List<Movies>>(json);
            return View(movies);
        }


        // Hiển thị danh sách phim từ API
        public async Task<IActionResult> ComingSoon()
        {
            var response = await _httpClient.GetAsync("https://localhost:7279/api/movies");
            if (!response.IsSuccessStatusCode)
            {
                return View(new List<Movies>());
            }

            var json = await response.Content.ReadAsStringAsync();
            var movies = JsonConvert.DeserializeObject<List<Movies>>(json);
            return View(movies);
        }

        // Hiển thị danh sách phim từ API
        public async Task<IActionResult> ChiTiet()
        {
            var response = await _httpClient.GetAsync("https://localhost:7279/api/movies");
            if (!response.IsSuccessStatusCode)
            {
                return View(new List<Movies>());
            }

            var json = await response.Content.ReadAsStringAsync();
            var movies = JsonConvert.DeserializeObject<List<Movies>>(json);
            return View(movies);
        }

        // Hiển thị thông tin chi tiết phim và đặt vé
        public async Task<IActionResult> Details(int id)
        {
            var response = await _httpClient.GetAsync($"https://localhost:7279/api/movies/{id}");
            if (!response.IsSuccessStatusCode)
            {
                return NotFound();
            }

            var json = await response.Content.ReadAsStringAsync();
            var movie = JsonConvert.DeserializeObject<Movies>(json);
            return View(movie);
        }

        public IActionResult SpecialTheaters()
        {
            return View();
        }
        public IActionResult Promotion()
        {
            return View();
        }

        public IActionResult Error()
        {
            var errorModel = new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            };
            return View(errorModel); // Đảm bảo model được truyền vào view
        }
    }
}
