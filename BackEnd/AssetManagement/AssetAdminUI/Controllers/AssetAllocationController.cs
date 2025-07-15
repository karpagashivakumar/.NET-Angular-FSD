using AssetAdminUI.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Http;

namespace AssetAdminUI.Controllers
{
    [Route("Admin/AssetAllocation")]
    public class AssetAllocationController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public AssetAllocationController(IHttpClientFactory httpClientFactory)
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

        [HttpGet("List")]
        public async Task<IActionResult> List()
        {
            var client = GetClient();
            var allocations = await client.GetFromJsonAsync<List<AssetAllocation>>("api/v1/AssetAllocation");
            return View(allocations);
        }

        [HttpGet("Create")]
        public IActionResult Create()
        {
            return View(new AssetAllocation());
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create(AssetAllocation model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var client = GetClient();
            var response = await client.PostAsJsonAsync("api/v1/AssetAllocation", model);

            TempData["ToastMessage"] = response.IsSuccessStatusCode ? "Allocation successful!" : "Failed to allocate asset.";
            TempData["ToastType"] = response.IsSuccessStatusCode ? "success" : "danger";

            return response.IsSuccessStatusCode ? RedirectToAction("List") : View(model);
        }
    }
}

