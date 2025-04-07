using Microsoft.AspNetCore.Mvc;
using MovieBookingMVC.ViewModels;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Collections.Generic;
using MovieBookingMVC.Models;

public class ShowtimesController : Controller
{
    private readonly HttpClient _httpClient;

    public ShowtimesController(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<IActionResult> Index()
    {
        // Lấy movieId từ cookie
        string movieId = Request.Cookies["selectedMovieId"];

        if (string.IsNullOrEmpty(movieId))
        {
            return RedirectToAction("Index", "Home"); // Nếu không có movieId, quay về trang chủ
        }

        // Gọi API backend để lấy danh sách suất chiếu theo movieId
        string apiUrl = $"https://localhost:7279/api/showtimes?movieId={movieId}";

        try
        {
            var response = await _httpClient.GetAsync(apiUrl);

            if (!response.IsSuccessStatusCode)
            {
                return View(new List<ShowtimeViewModel>()); // Nếu lỗi, trả về danh sách rỗng
            }

            var jsonData = await response.Content.ReadAsStringAsync();

            // Thiết lập JSON Serializer Options
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                Converters = { new DateTimeConverter() }
            };

            var showtimes = JsonSerializer.Deserialize<List<ShowtimeViewModel>>(jsonData, options);

            return View(showtimes ?? new List<ShowtimeViewModel>());
        }
        catch (JsonException ex)
        {
            Console.WriteLine($"JSON Deserialize Error: {ex.Message}");
            return View(new List<ShowtimeViewModel>()); // Trả về danh sách rỗng nếu lỗi
        }
    }
}