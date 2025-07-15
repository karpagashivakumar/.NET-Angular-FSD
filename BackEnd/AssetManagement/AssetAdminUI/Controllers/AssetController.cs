using AssetAdminUI.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Http;

namespace AssetAdminUI.Controllers
{
    [Route("Admin/Asset")]
    public class AssetController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public AssetController(IHttpClientFactory httpClientFactory)
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
        public async Task<IActionResult> List(string search, string category, string sortBy)
        {
            var client = GetClient();
            var assets = await client.GetFromJsonAsync<List<Asset>>("api/v1/Asset");

            // Filter by search
            if (!string.IsNullOrWhiteSpace(search))
            {
                search = search.ToLower();
                assets = assets.Where(a =>
                    (!string.IsNullOrEmpty(a.AssetName) && a.AssetName.ToLower().Contains(search)) ||
                    (!string.IsNullOrEmpty(a.AssetNo) && a.AssetNo.ToLower().Contains(search))
                ).ToList();
            }

            // Filter by category
            if (!string.IsNullOrWhiteSpace(category))
            {
                assets = assets.Where(a => a.AssetCategory?.Equals(category, StringComparison.OrdinalIgnoreCase) == true).ToList();
            }

            // Sorting
            assets = sortBy?.ToLower() switch
            {
                "value" => assets.OrderByDescending(a => a.AssetValue).ToList(),
                "date" => assets.OrderByDescending(a => a.ManufacturingDate).ToList(),
                "status" => assets.OrderBy(a => a.AssetStatus).ToList(),
                _ => assets
            };

            // Setup for UI
            ViewBag.Categories = assets
                .Select(a => a.AssetCategory)
                .Where(c => !string.IsNullOrEmpty(c))
                .Distinct()
                .ToList();

            ViewBag.SearchQuery = search;
            ViewBag.SelectedCategory = category;
            ViewBag.SortBy = sortBy;

            return View(assets);
        }



        [HttpGet("Create")]
        public IActionResult Create()
        {
            return View(new AssetCreate());
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create(Asset model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var client = GetClient();
            var response = await client.PostAsJsonAsync("api/v1/Asset", model);

            TempData["ToastMessage"] = response.IsSuccessStatusCode ? "Asset created successfully!" : "Failed to create asset.";
            TempData["ToastType"] = response.IsSuccessStatusCode ? "success" : "danger";

            return response.IsSuccessStatusCode ? RedirectToAction("List") : View(model);
        }

        [HttpGet("Edit/{id}")]
        public async Task<IActionResult> Edit(long id)
        {
            var client = GetClient();
            var asset = await client.GetFromJsonAsync<Asset>($"api/v1/Asset/{id}");

            // Map Asset to AssetEdit manually
            var assetEdit = new AssetEdit
            {
                AssetId = asset.AssetId,
                AssetNo = asset.AssetNo,
                AssetName = asset.AssetName,
                AssetCategory = asset.AssetCategory,
                AssetStatus = asset.AssetStatus,
                AssetValue = asset.AssetValue,
                ImageUrl = asset.ImageUrl
            };

            return View(assetEdit);
        }


        [HttpPost("Edit/{id}")]
        public async Task<IActionResult> Edit(long id, Asset model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var client = GetClient();
            var response = await client.PutAsJsonAsync($"api/v1/Asset/{id}", model);

            TempData["ToastMessage"] = response.IsSuccessStatusCode ? "Asset updated successfully!" : "Failed to update asset.";
            TempData["ToastType"] = response.IsSuccessStatusCode ? "success" : "danger";

            return response.IsSuccessStatusCode ? RedirectToAction("List") : View(model);
        }

        [HttpGet("Details/{id}")]
        public async Task<IActionResult> Details(long id)
        {
            var client = GetClient();
            var asset = await client.GetFromJsonAsync<Asset>($"api/v1/Asset/{id}");
            return View(asset);
        }


        [HttpPost("Delete/{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            var client = GetClient();
            var response = await client.DeleteAsync($"api/v1/Asset/{id}");

            TempData["ToastMessage"] = response.IsSuccessStatusCode ? "Asset deleted successfully!" : "Failed to delete asset.";
            TempData["ToastType"] = response.IsSuccessStatusCode ? "success" : "danger";

            return RedirectToAction("List");
        }
    }
}
