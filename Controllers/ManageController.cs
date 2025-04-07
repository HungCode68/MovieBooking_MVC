using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Collections.Generic;
using MovieBookingMVC.ViewModels;
using MovieBookingMVC.Models;
using System.Text;


namespace MovieBookingMVC.Controllers
{
    public class ManageController : Controller
    {
        private readonly HttpClient _httpClient;

        public ManageController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IActionResult> Movies()
        {
            var response = await _httpClient.GetAsync("https://localhost:7279/api/movies");
            if (!response.IsSuccessStatusCode) return View(new List<Movies>());
            var jsonData = await response.Content.ReadAsStringAsync();
            var movies = JsonConvert.DeserializeObject<List<Movies>>(jsonData);
            return View(movies);
        }

        public async Task<IActionResult> Showtimes()
        {
            var response = await _httpClient.GetAsync("https://localhost:7279/api/showtimes");
            if (!response.IsSuccessStatusCode) return View(new List<Showtime>());
            var jsonData = await response.Content.ReadAsStringAsync();
            var showtimes = JsonConvert.DeserializeObject<List<Showtime>>(jsonData);
            return View(showtimes);
        }

        public async Task<IActionResult> Seats(int showtimeId)
        {
            // Gọi API lấy danh sách suất chiếu
            var showtimeResponse = await _httpClient.GetAsync("https://localhost:7279/api/showtimes");
            if (showtimeResponse.IsSuccessStatusCode)
            {
                var showtimeJson = await showtimeResponse.Content.ReadAsStringAsync();
                var showtimes = JsonConvert.DeserializeObject<List<ShowtimeViewModel>>(showtimeJson);
                ViewBag.Showtimes = showtimes; // ✅ Đảm bảo ViewBag có dữ liệu
            }
            else
            {
                ViewBag.Showtimes = new List<ShowtimeViewModel>(); // ✅ Tránh null
            }

            // Gọi API lấy danh sách ghế theo suất chiếu
            var seatResponse = await _httpClient.GetAsync($"https://localhost:7279/api/seats/showtime/{showtimeId}");
            if (!seatResponse.IsSuccessStatusCode) return View(new List<Seat>());

            var seatJson = await seatResponse.Content.ReadAsStringAsync();
            var seats = JsonConvert.DeserializeObject<List<Seat>>(seatJson);
            return View(seats);
        }

        public async Task<IActionResult> Users()
        {
            var response = await _httpClient.GetAsync("https://localhost:7279/api/users");

            if (!response.IsSuccessStatusCode)
            {
                return View(new List<UserViewModel>()); // Trả về danh sách rỗng nếu API lỗi
            }

            var jsonData = await response.Content.ReadAsStringAsync();
            var users = JsonConvert.DeserializeObject<List<UserViewModel>>(jsonData);

            return View(users);
        }
        public async Task<IActionResult> UserDetails()
        {
            var response = await _httpClient.GetAsync("https://localhost:7279/api/userdetails/getusers");

            if (!response.IsSuccessStatusCode)
            {
                return View(new List<UserDetails>()); // Trả về danh sách rỗng nếu API lỗi
            }

            var jsonData = await response.Content.ReadAsStringAsync();
            var userDetails = JsonConvert.DeserializeObject<List<UserDetails>>(jsonData);

            return View(userDetails);
        }

        public async Task<IActionResult> GetMovie(int id)
        {
            var response = await _httpClient.GetAsync($"https://localhost:7279/api/movies/{id}");
            if (!response.IsSuccessStatusCode) return NotFound();

            var jsonData = await response.Content.ReadAsStringAsync();
            var movie = JsonConvert.DeserializeObject<Movies>(jsonData);
            return Json(movie);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteMovie(int id)
        {
            var response = await _httpClient.DeleteAsync($"https://localhost:7279/api/movies/{id}");

            if (!response.IsSuccessStatusCode)
            {
                return BadRequest("Không thể xóa phim.");
            }

            return Ok("Xóa phim thành công.");
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteSeat(int id)
        {
            var response = await _httpClient.DeleteAsync($"https://localhost:7279/api/seats/{id}");

            if (!response.IsSuccessStatusCode)
            {
                return BadRequest("Không thể xóa ghế.");
            }

            return Ok("Xóa ghế thành công.");
        }

        [HttpPut]
        public async Task<IActionResult> UpdateUserDetails(int id, UserDetails model)
        {
            var response = await _httpClient.PutAsJsonAsync($"https://localhost:7279/api/userdetails/update/{id}", model);
            if (!response.IsSuccessStatusCode)
            {
                return BadRequest("Không thể cập nhật thông tin.");
            }

            return Ok("Cập nhật thông tin thành công.");
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteUserDetail(int id)
        {
            var response = await _httpClient.DeleteAsync($"https://localhost:7279/api/userdetails/{id}");

            if (!response.IsSuccessStatusCode)
            {
                return BadRequest("Không thể xóa thông tin người dùng.");
            }

            return Ok("Xóa thành công.");
        }

        public async Task<IActionResult> Tickets()

        {

            var response = await _httpClient.GetAsync("https://localhost:7279/api/BookingPayment/all");

            if (!response.IsSuccessStatusCode) return View(new List<BookingPaymentViewModel>());



            var jsonData = await response.Content.ReadAsStringAsync();

            var bookings = JsonConvert.DeserializeObject<List<BookingPaymentViewModel>>(jsonData);



            return View(bookings);

        }

        [HttpGet("TicketsByDate")]
        public async Task<IActionResult> TicketsByDate([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            var response = await _httpClient.GetAsync($"https://localhost:7279/api/tickets/filter?startDate={startDate:yyyy-MM-dd}&endDate={endDate:yyyy-MM-dd}");

            if (!response.IsSuccessStatusCode) return View(new List<Ticket>());

            var jsonData = await response.Content.ReadAsStringAsync();
            var tickets = JsonConvert.DeserializeObject<List<Ticket>>(jsonData);
            return View("Tickets", tickets);
        }

        public async Task<IActionResult> Statistical(DateTime? startDate, DateTime? endDate)
        {
            string url = "https://localhost:7279/api/statistical";
            if (startDate.HasValue && endDate.HasValue)
            {
                url += $"?startDate={startDate.Value:yyyy-MM-dd}&endDate={endDate.Value:yyyy-MM-dd}";
            }

            var response = await _httpClient.GetAsync(url);
            if (!response.IsSuccessStatusCode) return View(new Statistical());

            var jsonData = await response.Content.ReadAsStringAsync();
            var statistics = JsonConvert.DeserializeObject<Statistical>(jsonData);

            return View(statistics);
        }

        // Hiển thị danh sách phòng
        public async Task<IActionResult> Rooms()
        {
            var response = await _httpClient.GetAsync("https://localhost:7279/api/cinemas");
            if (!response.IsSuccessStatusCode) return View(new List<Cinema>());
            var jsonData = await response.Content.ReadAsStringAsync();
            var cinemas = JsonConvert.DeserializeObject<List<Cinema>>(jsonData);
            return View(cinemas);
        }

        [HttpPost]
        public async Task<IActionResult> CreateRoom([FromBody] Cinema room)
        {
            var content = new StringContent(JsonConvert.SerializeObject(room), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("https://localhost:7279/api/cinemas", content);

            if (!response.IsSuccessStatusCode)
            {
                var errorMessage = await response.Content.ReadAsStringAsync();
                return BadRequest(errorMessage);
            }

            // Sau khi tạo phòng thành công, lấy ID của phòng vừa tạo
            var createdRoom = JsonConvert.DeserializeObject<Cinema>(await response.Content.ReadAsStringAsync());

            // Gọi API để tạo ghế mặc định cho phòng
            var createSeatsResponse = await _httpClient.PostAsync($"https://localhost:7279/api/seats/create-default/{createdRoom.Id}", null);

            if (!createSeatsResponse.IsSuccessStatusCode)
            {
                return BadRequest("Phòng tạo thành công, nhưng lỗi khi tạo ghế.");
            }

            return Ok("Tạo phòng và ghế thành công.");
        }

        public async Task<IActionResult> GetRoom(int id)
        {
            var response = await _httpClient.GetAsync($"https://localhost:7279/api/cinemas/{id}");
            if (!response.IsSuccessStatusCode) return NotFound();

            var jsonData = await response.Content.ReadAsStringAsync();
            var cinema = JsonConvert.DeserializeObject<Cinema>(jsonData);

            return Json(new
            {
                id = cinema.Id,
                name = cinema.Name,
                numberRoom = cinema.NumberRoom
            });
        }

        [HttpPut]
        public async Task<IActionResult> UpdateRoom(int id, [FromBody] Cinema updatedCinema)
        {
            if (updatedCinema == null || id != updatedCinema.Id)
            {
                return BadRequest("Dữ liệu không hợp lệ.");
            }

            var content = new StringContent(JsonConvert.SerializeObject(updatedCinema), Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync($"https://localhost:7279/api/cinemas/{id}", content);

            if (!response.IsSuccessStatusCode)
            {
                return BadRequest("Không thể cập nhật phòng.");
            }

            return Ok("Cập nhật phòng thành công.");
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteRoom(int id)
        {
            var response = await _httpClient.DeleteAsync($"https://localhost:7279/api/cinemas/{id}");

            if (!response.IsSuccessStatusCode)
            {
                return BadRequest("Không thể xóa phòng.");
            }

            return Ok("Xóa phòng thành công.");
        }




        [HttpPost]
        public async Task<IActionResult> CreateShowtime([FromBody] Showtime showtime)
        {
            var content = new StringContent(JsonConvert.SerializeObject(showtime), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("https://localhost:7279/api/showtimes/create", content);

            if (!response.IsSuccessStatusCode)
            {
                var errorMessage = await response.Content.ReadAsStringAsync();
                return BadRequest(errorMessage);
            }

            return Ok("Thêm suất chiếu thành công.");
        }

        [HttpPost]
        public async Task<IActionResult> CreateMovie([FromBody] Movies movie)
        {
            var content = new StringContent(JsonConvert.SerializeObject(movie), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("https://localhost:7279/api/movies", content);

            if (!response.IsSuccessStatusCode)
            {
                var errorMessage = await response.Content.ReadAsStringAsync();
                return BadRequest(errorMessage);
            }

            return Ok("Thêm phim thành công.");
        }

        public async Task<IActionResult> RevenueByMovie()
        {
            var response = await _httpClient.GetAsync("https://localhost:7279/api/statistical/revenue-by-movie");
            if (!response.IsSuccessStatusCode) return View(new List<object>());

            var jsonData = await response.Content.ReadAsStringAsync();
            var revenueData = JsonConvert.DeserializeObject<List<object>>(jsonData);
            return View(revenueData);
        }

        public async Task<IActionResult> HighestRevenueMovie()
        {
            var response = await _httpClient.GetAsync("https://localhost:7279/api/statistical/highest-revenue-movie");
            if (!response.IsSuccessStatusCode) return View(null);

            var jsonData = await response.Content.ReadAsStringAsync();
            var highestRevenueMovie = JsonConvert.DeserializeObject<object>(jsonData);
            return View(highestRevenueMovie);
        }





    }
}