using AssetAdminUI.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AssetAdminUI.Controllers
{
    [Route("Admin")]
    public class AdminController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public AdminController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        private HttpClient GetClient()
        {
            var client = _httpClientFactory.CreateClient("ApiClient");
            var token = HttpContext.Session.GetString("JWToken");
            if (!string.IsNullOrEmpty(token))
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            return client;
        }

        [HttpGet("Dashboard")]
        public async Task<IActionResult> Dashboard()
        {
            var username = HttpContext.Session.GetString("Username");
            ViewBag.Username = username;

            var client = GetClient();

            try
            {
                var users = await client.GetFromJsonAsync<List<User>>("api/v1/User");
                var assets = await client.GetFromJsonAsync<List<Asset>>("api/v1/Asset");
                var allocations = await client.GetFromJsonAsync<List<AssetAllocation>>("api/v1/AssetAllocation");
                var audits = await client.GetFromJsonAsync<List<AssetAudit>>("api/v1/AssetAudit");
                var serviceRequests = await client.GetFromJsonAsync<List<ServiceRequestView>>("api/v1/ServiceRequest");
                var assetRequests = await client.GetFromJsonAsync<List<AssetRequestView>>("api/v1/AssetRequest");

                ViewBag.TotalUsers = users?.Count ?? 0;
                ViewBag.TotalAssets = assets?.Count ?? 0;
                ViewBag.TotalAllocations = allocations?.Count ?? 0;
                ViewBag.TotalAudits = audits?.Count ?? 0;
                ViewBag.TotalServiceRequests = serviceRequests?.Count ?? 0;
                ViewBag.TotalAssetRequests = assetRequests?.Count ?? 0;
            }
            catch
            {
                ViewBag.TotalUsers = 0;
                ViewBag.TotalAssets = 0;
                ViewBag.TotalAllocations = 0;
                ViewBag.TotalAudits = 0;
                ViewBag.TotalServiceRequests = 0;
                ViewBag.TotalAssetRequests = 0;
            }

            return View("Dashboard");
        }

        [HttpGet("UserList")]
        public async Task<IActionResult> UserList()
        {
            var client = GetClient();
            var users = await client.GetFromJsonAsync<List<User>>("api/v1/User");
            return View(users);
        }

        [HttpGet("CreateUser")]
        public IActionResult CreateUser()
        {
            return View(new CreateUser());
        }

        [HttpPost("CreateUser")]
        public async Task<IActionResult> CreateUser(CreateUser model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var client = GetClient();
            var response = await client.PostAsJsonAsync("api/v1/User", model);

            if (response.IsSuccessStatusCode)
            {
                TempData["ToastMessage"] = "User created successfully!";
                TempData["ToastType"] = "success";
                return RedirectToAction("UserList");
            }

            TempData["ToastMessage"] = "Failed to create user.";
            TempData["ToastType"] = "danger";
            return View(model);
        }

        [HttpPost("Activate/{id}")]
        public async Task<IActionResult> Activate(long id)
        {
            var client = GetClient();
            var response = await client.PutAsync($"api/v1/User/{id}/activate", null);

            TempData["ToastMessage"] = response.IsSuccessStatusCode
                ? "User Activated Successfully!"
                : "Failed to activate user.";

            TempData["ToastType"] = response.IsSuccessStatusCode ? "success" : "danger";
            return RedirectToAction("UserList");
        }

        [HttpPost("Deactivate/{id}")]
        public async Task<IActionResult> Deactivate(long id)
        {
            var client = GetClient();
            var response = await client.PutAsync($"api/v1/User/{id}/deactivate", null);

            TempData["ToastMessage"] = response.IsSuccessStatusCode
                ? "User Deactivated Successfully!"
                : "Failed to deactivate user.";

            TempData["ToastType"] = response.IsSuccessStatusCode ? "success" : "danger";
            return RedirectToAction("UserList");
        }

        [HttpGet("Profile")]
        public async Task<IActionResult> Profile()
        {
            var client = GetClient();
            var profile = await client.GetFromJsonAsync<User>("api/v1/User/me");
            return View(profile);
        }

        [HttpGet("EditProfile")]
        public async Task<IActionResult> EditProfile()
        {
            var client = GetClient();
            var user = await client.GetFromJsonAsync<UserEdit>("api/v1/User/me");
            return View(user);
        }

        [HttpPost("EditProfile")]
        public async Task<IActionResult> EditProfile(UserEdit updatedUser)
        {
            if (!ModelState.IsValid)
                return View(updatedUser);

            var client = GetClient();

            var userDto = new
            {
                userId = updatedUser.UserId,
                username = updatedUser.Username,
                email = updatedUser.Email,
                firstName = updatedUser.FirstName,
                lastName = updatedUser.LastName,
                contactNumber = updatedUser.ContactNumber,
                address = updatedUser.Address
            };

            var response = await client.PutAsJsonAsync($"api/v1/User/{updatedUser.UserId}", userDto);

            var content = await response.Content.ReadAsStringAsync();
            Console.WriteLine("Update Response: " + content);

            if (response.IsSuccessStatusCode)
            {
                TempData["ToastMessage"] = "Profile Updated Successfully!";
                TempData["ToastType"] = "success";
                return RedirectToAction("Profile");
            }

            TempData["ToastMessage"] = "Failed to update profile.";
            TempData["ToastType"] = "danger";
            return RedirectToAction("EditProfile");
        }

        [HttpGet("ChangePassword")]
        public IActionResult ChangePassword()
        {
            return View(new ChangePassword());
        }

        [HttpPost("ChangePassword")]
        public async Task<IActionResult> ChangePassword(ChangePassword dto)
        {
            if (!ModelState.IsValid)
                return View(dto);

            var client = GetClient();
            var response = await client.PostAsJsonAsync("api/v1/User/change-password", dto);

            if (response.IsSuccessStatusCode)
            {
                TempData["ToastMessage"] = "Password changed successfully!";
                TempData["ToastType"] = "success";
                return RedirectToAction("Profile");
            }

            TempData["ToastMessage"] = "Failed to change password.";
            TempData["ToastType"] = "danger";
            return View(dto);
        }
    }
}
