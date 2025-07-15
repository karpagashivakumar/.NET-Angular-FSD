using AssetAdminUI.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AssetAdminUI.Controllers
{
    [Route("Admin/AssetAudit")]
    public class AssetAuditController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public AssetAuditController(IHttpClientFactory httpClientFactory)
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
        public async Task<IActionResult> List(string status = "")
        {
            var client = GetClient();
            string url = string.IsNullOrEmpty(status) ? "api/v1/AssetAudit" : "api/v1/AssetAudit/pending";

            var audits = await client.GetFromJsonAsync<List<AssetAuditView>>(url);
            ViewBag.SelectedStatus = status;
            return View(audits);
        }

        [HttpGet("Create")]
        public IActionResult Create()
        {
            return View(new AssetAuditCreate());
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create(AssetAuditCreate audit)
        {
            if (!ModelState.IsValid) return View(audit);

            var client = GetClient();
            var response = await client.PostAsJsonAsync("api/v1/AssetAudit/with-dto", audit);

            if (response.IsSuccessStatusCode)
            {
                TempData["ToastMessage"] = "Audit request created.";
                TempData["ToastType"] = "success";
                return RedirectToAction("List");
            }

            TempData["ToastMessage"] = "Failed to create audit.";
            TempData["ToastType"] = "danger";
            return View(audit);
        }

        [HttpPost("Complete")]
        public async Task<IActionResult> Complete(AssetAuditComplete dto)
        {
            var client = GetClient();
            var response = await client.PutAsJsonAsync($"api/v1/AssetAudit/complete/{dto.AuditId}", dto.Notes);

            TempData["ToastMessage"] = response.IsSuccessStatusCode
                ? "Audit completed."
                : "Failed to complete audit.";
            TempData["ToastType"] = response.IsSuccessStatusCode ? "success" : "danger";

            return RedirectToAction("List");
        }
    }
}
