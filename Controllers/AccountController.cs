using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MovieBookingMVC.Models;
using Newtonsoft.Json;
using System.Text;
using System.Net.Http.Headers;

namespace MovieBookingMVC.Controllers
{
    public class AccountController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AccountController(HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClient;
            _httpContextAccessor = httpContextAccessor;
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(UserViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Message = "Vui lòng nhập đầy đủ thông tin.";
                return View(model);
            }

            var json = JsonConvert.SerializeObject(model);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("https://localhost:7279/api/account/register", content);
            var responseMessage = await response.Content.ReadAsStringAsync();

            Console.WriteLine("API Response: " + responseMessage);

            if (response.IsSuccessStatusCode)
            {
                ViewBag.Message = "Đăng ký thành công.";
            }
            else
            {
                ViewBag.Message = "Lỗi: " + responseMessage;
            }

            return View(model);
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Message = "Vui lòng nhập email và mật khẩu.";
                return View(model);
            }

            var jsonContent = JsonConvert.SerializeObject(model);
            var response = await _httpClient.PostAsync("https://localhost:7279/api/account/login",
                new StringContent(jsonContent, Encoding.UTF8, "application/json"));

            var responseMessage = await response.Content.ReadAsStringAsync();
            Console.WriteLine("API Response: " + responseMessage);

            if (response.IsSuccessStatusCode)
            {
                var jsonResponse = JsonConvert.DeserializeObject<dynamic>(responseMessage);
                if (jsonResponse?.token != null)
                {
                    var cookieOptions = new CookieOptions
                    {
                        HttpOnly = true,
                        Expires = DateTime.UtcNow.AddHours(1),
                        SameSite = SameSiteMode.None,
                        Secure = true
                    };

                    Response.Cookies.Append("AuthToken", jsonResponse.token.ToString(), cookieOptions);
                    Response.Cookies.Append("FullName", jsonResponse.user.fullName.ToString(), cookieOptions);
                    Response.Cookies.Append("UserId", jsonResponse.user.id.ToString(), cookieOptions);
                    Response.Cookies.Append("UserRole", jsonResponse.user.role.ToString(), cookieOptions); // Lưu Role

                    Console.WriteLine($"✅ Cookie UserId đã được lưu: {jsonResponse.user.id}");
                    Console.WriteLine($"✅ Cookie Role đã được lưu: {jsonResponse.user.role}");

                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ViewBag.Message = "Lỗi: API không trả về token.";
                }
            }
            else
            {
                ViewBag.Message = "Lỗi đăng nhập: " + responseMessage;
            }

            return View(model);
        }

        public IActionResult Logout()
        {
            Response.Cookies.Delete("AuthToken");
            Response.Cookies.Delete("FullName");
            Response.Cookies.Delete("UserId");
            Response.Cookies.Delete("UserRole"); // Xóa Role khi đăng xuất

            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> Profile()
        {
            var userIdString = Request.Cookies["UserId"];
            if (string.IsNullOrEmpty(userIdString) || !int.TryParse(userIdString, out int userId))
            {
                Console.WriteLine("❌ Cookie UserId không tồn tại hoặc không hợp lệ.");
                return RedirectToAction("Login");
            }

            Console.WriteLine($"✅ Đang lấy thông tin UserId: {userId}");

            // Lấy thông tin UserDetails
            var userDetailsResponse = await _httpClient.GetAsync($"https://localhost:7279/api/userdetails/{userId}");
            Console.WriteLine($"📌 API UserDetails Response Status: {userDetailsResponse.StatusCode}");

            if (!userDetailsResponse.IsSuccessStatusCode)
            {
                ViewBag.Message = "Không thể lấy thông tin người dùng.";
                return View();
            }

            var userDetailsJson = await userDetailsResponse.Content.ReadAsStringAsync();
            Console.WriteLine($"📌 API UserDetails Response Body: {userDetailsJson}");

            var userDetails = JsonConvert.DeserializeObject<UserDetails>(userDetailsJson);

            if (userDetails == null)
            {
                ViewBag.Message = "Dữ liệu UserDetails bị lỗi hoặc rỗng.";
                return View();
            }

            // Lấy thông tin User riêng biệt vì nó là [NotMapped]
            var userResponse = await _httpClient.GetAsync($"https://localhost:7279/api/users/{userId}");
            Console.WriteLine($"📌 API User Response Status: {userResponse.StatusCode}");

            if (userResponse.IsSuccessStatusCode)
            {
                var userJson = await userResponse.Content.ReadAsStringAsync();
                Console.WriteLine($"📌 API User Response Body: {userJson}");

                var user = JsonConvert.DeserializeObject<UserViewModel>(userJson);
                userDetails.User = user; // Gán dữ liệu User vào UserDetails
            }
            else
            {
                Console.WriteLine("⚠️ Không thể lấy thông tin người dùng.");
                userDetails.User = new UserViewModel { FullName = "Không xác định", Email = "Không có email" };
            }

            return View(userDetails);
        }

        public async Task<IActionResult> TicketHistory()
        {
            var userIdString = Request.Cookies["UserId"];
            if (string.IsNullOrEmpty(userIdString) || !int.TryParse(userIdString, out int userId))
            {
                return RedirectToAction("Login");
            }

            var response = await _httpClient.GetAsync($"https://localhost:7279/api/ticket/user/{userId}");
            if (!response.IsSuccessStatusCode)
            {
                ViewBag.ErrorMessage = "Không có vé nào được đặt.";
                return View(new List<TicketViewModel>());
            }

            var tickets = await response.Content.ReadFromJsonAsync<List<TicketViewModel>>();
            return View(tickets);
        }

        public async Task<IActionResult> TransactionHistory()
        {
            var userIdString = Request.Cookies["UserId"];
            if (string.IsNullOrEmpty(userIdString) || !int.TryParse(userIdString, out int userId))
            {
                return RedirectToAction("Login");
            }

            var response = await _httpClient.GetAsync($"https://localhost:7279/api/BookingPayment/user/{userId}");

            if (!response.IsSuccessStatusCode)
            {
                ViewBag.ErrorMessage = "Không thể lấy lịch sử giao dịch.";
                return View(new List<BookingPaymentViewModel>());
            }

            var transactions = await response.Content.ReadFromJsonAsync<List<BookingPaymentViewModel>>();
            return View(transactions ?? new List<BookingPaymentViewModel>());
        }


        private async Task<List<TicketViewModel>> GetUserTransactionHistory()
        {
            var userIdString = Request.Cookies["UserId"];
            if (string.IsNullOrEmpty(userIdString) || !int.TryParse(userIdString, out int userId))
            {
                Console.WriteLine("❌ Không lấy được UserId từ cookie.");
                return new List<TicketViewModel>();
            }

            Console.WriteLine($"✅ Lấy lịch sử giao dịch của UserId: {userId}");

            var response = await _httpClient.GetAsync($"https://localhost:7279/api/ticket/user/{userId}");
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine("❌ API không trả về dữ liệu hợp lệ.");
                return new List<TicketViewModel>();
            }

            var tickets = await response.Content.ReadFromJsonAsync<List<TicketViewModel>>();
            return tickets ?? new List<TicketViewModel>(); // Trả về danh sách rỗng nếu không có dữ liệu
        }
        public IActionResult Movies()
        {
            return View();
        }

        
    }
}
