using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using MovieBookingMVC.ViewModels;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
using MovieBookingMVC.Models;

namespace MovieBookingMVC.Controllers
{
    public class QuickBookingController : Controller
    {
        private readonly HttpClient _httpClient;

        public QuickBookingController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IActionResult> Index(string date, int? cinemaId)
        {
            var model = new QuickBookingViewModel
            {
                SelectedDate = date ?? DateTime.Today.ToString("yyyy-MM-dd"),
                SelectedCinemaId = cinemaId ?? 0,
                Movies = new List<Movies>()
            };

            // Lấy danh sách rạp chiếu
            var response = await _httpClient.GetAsync("https://localhost:7279/api/cinemas");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                model.Cinemas = JsonConvert.DeserializeObject<List<Cinema>>(json);
            }

            // Nếu đã chọn ngày và rạp, lấy danh sách phim
            if (!string.IsNullOrEmpty(date) && cinemaId.HasValue)
            {
                var showtimeResponse = await _httpClient.GetAsync($"https://localhost:7279/api/showtimes/cinema/{cinemaId}?date={date}");
                if (showtimeResponse.IsSuccessStatusCode)
                {
                    var showtimeJson = await showtimeResponse.Content.ReadAsStringAsync();
                    model.Movies = JsonConvert.DeserializeObject<List<Movies>>(showtimeJson);
                }
            }

            return View(model);
        }

        public async Task<IActionResult> SelectShowtime(int movieId)
        {
            var response = await _httpClient.GetAsync($"https://localhost:7279/api/showtimes/movie/{movieId}");
            if (!response.IsSuccessStatusCode)
            {
                return View(new List<ShowtimeViewModel>());
            }

            var json = await response.Content.ReadAsStringAsync();
            var showtimes = JsonConvert.DeserializeObject<List<ShowtimeViewModel>>(json);

            return View("SelectShowtime", showtimes);
        }
        public async Task<IActionResult> GetCinemas()
        {
            var response = await _httpClient.GetAsync("https://localhost:7279/api/cinemas");
            if (!response.IsSuccessStatusCode)
            {
                return Json(new List<Cinema>());
            }

            var json = await response.Content.ReadAsStringAsync();
            var cinemas = JsonConvert.DeserializeObject<List<Cinema>>(json);
            return Json(cinemas);
        }

    }
}
