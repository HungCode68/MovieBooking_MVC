using Microsoft.AspNetCore.Mvc;
using MovieBookingMVC.Models;
using System.Collections.Generic;

namespace MovieBookingMVC.Controllers
{
    public class MoviesController : Controller
    {
        private readonly HttpClient _httpClient;

        public MoviesController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<IActionResult> NowShowing()
        {
            var response = await _httpClient.GetAsync("https://localhost:7279/api/movies");
            if (!response.IsSuccessStatusCode)
            {
                ViewBag.Message = "Không thể lấy danh sách phim.";
                return View(new List<Movies>());
            }

            var movies = await response.Content.ReadFromJsonAsync<List<Movies>>();
            return View(movies);
        }

        public IActionResult ComingSoon()
        {
            return View();
        }

        public async Task<IActionResult> Details(int id)
        {
            // Đọc cookie SelectedMovieId
            var selectedMovieId = Request.Cookies["SelectedMovieId"];
            if (string.IsNullOrEmpty(selectedMovieId) || !int.TryParse(selectedMovieId, out int movieId))
            {
                movieId = id; // Nếu không có cookie, dùng id từ URL
            }

            var response = await _httpClient.GetAsync($"https://localhost:7279/api/movies/{movieId}");
            if (!response.IsSuccessStatusCode)
            {
                return NotFound();
            }

            var movie = await response.Content.ReadFromJsonAsync<Movies>();
            return View(movie);
        }

    }
}
