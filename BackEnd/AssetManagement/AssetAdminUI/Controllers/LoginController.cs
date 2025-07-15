using AssetAdminUI.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace AssetAdminUI.Controllers
{
    public class LoginController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public LoginController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View(new UserLogin());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(UserLogin login)
        {
            if (!ModelState.IsValid)
                return View(login);

            //// CAPTCHA Validation
            //var captchaResponse = Request.Form["g-recaptcha-response"];
            //var secretKey = "6Lfpgn8rAAAAALzItru0kVTHrbfUf06PWVXLkL7I";
            //var verifyUrl = $"https://www.google.com/recaptcha/api/siteverify?secret={secretKey}&response={captchaResponse}";

            //using var httpClient = new HttpClient();
            //var verifyResponse = await httpClient.PostAsync(verifyUrl, null);
            //var verifyJson = await verifyResponse.Content.ReadFromJsonAsync<CaptchaVerificationResponse>();

            //if (verifyJson == null || !verifyJson.success)
            //{
            //    ViewBag.Error = "CAPTCHA verification failed. Please try again.";
            //    return View(login);
            //}

            try
            {
                var client = _httpClientFactory.CreateClient("ApiClient");
                var response = await client.PostAsJsonAsync("api/v1/Auth/login", login);

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<TokenResponse>();

                    if (result.role.ToLower() != "admin")
                    {
                        ViewBag.Error = "Only admins can access this panel.";
                        return View(login);
                    }

                    // Secure session handling
                    HttpContext.Session.Clear();
                    HttpContext.Session.SetString("JWToken", result.token);
                    HttpContext.Session.SetString("Username", result.username);
                    HttpContext.Session.SetString("Role", result.role);

                    return RedirectToAction("Dashboard", "Admin");
                }

                ViewBag.Error = "Invalid credentials or inactive user.";
                return View(login);
            }
            catch (Exception ex)
            {
                ViewBag.Error = $"Login failed: {ex.Message}";
                return View(login);
            }
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }
    }

    public class TokenResponse
    {
        public string token { get; set; }
        public string username { get; set; }
        public string role { get; set; }
    }

    public class CaptchaVerificationResponse
    {
        public bool success { get; set; }
        public string challenge_ts { get; set; }
        public string hostname { get; set; }
    }
}
