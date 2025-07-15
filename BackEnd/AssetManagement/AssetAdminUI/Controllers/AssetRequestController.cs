using AssetAdminUI.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Net.Http.Json;

namespace AssetAdminUI.Controllers
{
    public class AssetRequestController : Controller
    {
        private readonly HttpClient _client;

        public AssetRequestController(IHttpClientFactory httpClientFactory)
        {
            _client = httpClientFactory.CreateClient("ApiClient");
        }

        public async Task<IActionResult> List()
        {
            var token = HttpContext.Session.GetString("JWToken");

            if (string.IsNullOrEmpty(token))
            {
                TempData["ToastMessage"] = "Unauthorized access. Please login again.";
                TempData["ToastType"] = "danger";
                return RedirectToAction("Index", "Login");
            }

            _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            try
            {
                var requests = await _client.GetFromJsonAsync<List<AssetRequestView>>("api/v1/AssetRequest");
                return View(requests);
            }
            catch (HttpRequestException ex)
            {
                TempData["ToastMessage"] = "Unauthorized or failed to fetch asset requests.";
                TempData["ToastType"] = "danger";
                return RedirectToAction("Index", "Login");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Approve(long id)
        {
            var token = HttpContext.Session.GetString("JWToken");
            _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var response = await _client.PutAsync($"api/v1/AssetRequest/{id}/approve", null);
            TempData["ToastMessage"] = response.IsSuccessStatusCode ? "Request approved successfully." : "Failed to approve request.";
            TempData["ToastType"] = response.IsSuccessStatusCode ? "success" : "danger";
            return RedirectToAction("List");
        }

        [HttpPost]
        public async Task<IActionResult> Reject(long id, string reason)
        {
            var token = HttpContext.Session.GetString("JWToken");
            _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var response = await _client.PutAsync($"api/v1/AssetRequest/{id}/reject?reason={reason}", null);
            TempData["ToastMessage"] = response.IsSuccessStatusCode ? "Request rejected." : "Failed to reject request.";
            TempData["ToastType"] = response.IsSuccessStatusCode ? "warning" : "danger";
            return RedirectToAction("List");
        }

    }
}

