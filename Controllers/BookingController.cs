using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;
using MovieBookingMVC.Models;
using MovieBookingMVC.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using MovieBookingMVC.Services;
using MovieBookingMVC.Data;
using MovieBookingMVC.ViewModels;
using MovieBookingMVC.Models; // Nếu PaymentRequest nằm trong Models
using Microsoft.EntityFrameworkCore;
using ZXing;
using ZXing.Common;
using ZXing.Rendering;
using SkiaSharp;
using Microsoft.Extensions.Configuration;






namespace MovieBookingMVC.Controllers
{
    public class BookingController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly ISeatService _seatService;
        private readonly IShowtimeService _showtimeService; // Thêm service lấy thông tin suất chiếu
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        public BookingController(HttpClient httpClient, ISeatService seatService, IShowtimeService showtimeService, ApplicationDbContext context, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _seatService = seatService;
            _showtimeService = showtimeService;
            _context = context;
            _configuration = configuration;
        }



        [HttpPost]
        public async Task<IActionResult> BookTicket(int showtimeId, [FromBody] List<int> selectedSeats)
        {
            if (selectedSeats == null || !selectedSeats.Any())
            {
                return BadRequest("Bạn chưa chọn ghế!");
            }

            var userIdString = Request.Cookies["UserId"];
            if (string.IsNullOrEmpty(userIdString) || !int.TryParse(userIdString, out int userId))
            {
                return RedirectToAction("Login", "Account");
            }

            var showtime = _showtimeService.GetShowtimeDetails(showtimeId);
            if (showtime == null)
            {
                return BadRequest("Suất chiếu không tồn tại!");
            }

            decimal totalAmount = selectedSeats.Count * showtime.Price;

            var request = new
            {
                UserId = userId, // Lấy UserId từ Cookie
                ShowtimeId = showtimeId,
                SeatIds = selectedSeats,
                Amount = totalAmount,
                PaymentMethod = "Momo"
            };

            var json = JsonConvert.SerializeObject(request);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("https://localhost:7279/api/bookingpayment", content);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Success");
            }

            return View("Error");
        }


        public async Task<IActionResult> Checkout(int showtimeId, List<int> seatIds, decimal totalPrice)
        {
            if (seatIds == null || !seatIds.Any())
            {
                return RedirectToAction("SeatSelection", new { showtimeId });
            }

            var showtime = _showtimeService.GetShowtimeDetails(showtimeId);
            if (showtime == null)
            {
                return NotFound("Suất chiếu không tồn tại!");
            }

            var model = new CheckoutViewModel
            {
                ShowtimeId = showtimeId,
                MovieTitle = showtime.MovieTitle,
                CinemaRoom = showtime.CinemaRoom,
                Showtime = showtime.Showtime,
                SeatNumbers = _seatService.GetSeatNumbersByIds(seatIds),
                TotalPrice = totalPrice,
                SelectedSeats = seatIds
            };

            HttpContext.Session.SetString("CheckoutData", JsonConvert.SerializeObject(model));

            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> ProcessPayment([FromBody] PaymentRequest request)
        {
            if (request == null || request.SelectedSeats == null || !request.SelectedSeats.Any())
            {
                return BadRequest("Thông tin đặt vé không hợp lệ!");
            }

            var userIdString = Request.Cookies["UserId"];
            if (string.IsNullOrEmpty(userIdString) || !int.TryParse(userIdString, out int userId))
            {
                return RedirectToAction("Login", "Account");
            }

            var showtime = _showtimeService.GetShowtimeDetails(request.ShowtimeId);
            if (showtime == null)
            {
                return BadRequest("Suất chiếu không tồn tại!");
            }

            decimal totalAmount = request.TotalPrice;

            var paymentData = new
            {
                UserId = userId, // Lấy UserId từ Cookie
                ShowtimeId = request.ShowtimeId,
                SeatIds = request.SelectedSeats,
                Amount = totalAmount,
                PaymentMethod = request.PaymentMethod
            };

            var json = JsonConvert.SerializeObject(paymentData);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("https://localhost:7279/api/bookingpayment", content);
            if (response.IsSuccessStatusCode)
            {
                HttpContext.Session.SetString("CheckoutData", JsonConvert.SerializeObject(new CheckoutViewModel
                {
                    ShowtimeId = request.ShowtimeId,
                    MovieTitle = showtime.MovieTitle,
                    CinemaRoom = showtime.CinemaRoom,
                    Showtime = showtime.Showtime,
                    SeatNumbers = _seatService.GetSeatNumbersByIds(request.SelectedSeats),
                    TotalPrice = totalAmount,
                    SelectedSeats = request.SelectedSeats
                }));

                return RedirectToAction("Success", new { totalPrice = totalAmount });
            }

            return View("Error");
        }



        public IActionResult Success()
        {
            var checkoutJson = HttpContext.Session.GetString("CheckoutData");

            if (!string.IsNullOrEmpty(checkoutJson))
            {
                var model = JsonConvert.DeserializeObject<CheckoutViewModel>(checkoutJson);
                return View(model);
            }

            return RedirectToAction("Index", "Home");
        }



        public IActionResult SeatSelection(int showtimeId)
        {
            // Lấy thông tin suất chiếu từ service
            var showtime = _showtimeService.GetShowtimeDetails(showtimeId);

            // Nếu không tìm thấy suất chiếu, trả về lỗi 404
            if (showtime == null)
            {
                return NotFound("Suất chiếu không tồn tại.");
            }

            // Lấy danh sách ghế ngồi cho suất chiếu này
            var seatLayout = _seatService.GetSeatsByShowtime(showtimeId);

            // Tạo model cho view
            var model = new SeatSelectionViewModel
            {
                ShowtimeId = showtimeId,
                MovieTitle = showtime.MovieTitle,  // Lấy tên phim
                CinemaRoom = showtime.CinemaRoom,  // Lấy số phòng chiếu
                Showtime = showtime.Showtime,      // Lấy thời gian chiếu (kiểu DateTime)
                Price = showtime.Price,            // Giá vé
                SeatLayout = seatLayout
            };

            return View(model);
        }




    }
}
