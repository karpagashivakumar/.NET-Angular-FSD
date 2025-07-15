using AssetAdminUI.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Net.Http.Json;

namespace AssetAdminUI.Controllers
{
    [Route("Admin/ServiceRequest")]
    public class ServiceRequestController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public ServiceRequestController(IHttpClientFactory httpClientFactory)
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
            var data = await client.GetFromJsonAsync<List<ServiceRequestView>>("api/v1/ServiceRequest");
            return View(data);
        }

        [HttpPost("Assign")]
        public async Task<IActionResult> Assign(ServiceRequestAssign model)
        {
            var client = GetClient();
            var response = await client.PutAsync($"api/v1/ServiceRequest/{model.ServiceRequestId}/assign?assignedTo={model.AssignedTo}", null);

            TempData["ToastMessage"] = response.IsSuccessStatusCode ? "Assigned successfully" : "Failed to assign.";
            TempData["ToastType"] = response.IsSuccessStatusCode ? "success" : "danger";
            return RedirectToAction("List");
        }

        [HttpPost("Resolve")]
        public async Task<IActionResult> Resolve(ServiceRequestResolve model)
        {
            var client = GetClient();
            var response = await client.PutAsJsonAsync($"api/v1/ServiceRequest/{model.ServiceRequestId}/resolve?notes={model.Notes}", model);

            TempData["ToastMessage"] = response.IsSuccessStatusCode ? "Resolved successfully" : "Failed to resolve.";
            TempData["ToastType"] = response.IsSuccessStatusCode ? "success" : "danger";
            return RedirectToAction("List");
        }

        [HttpPost("Close")]
        public async Task<IActionResult> Close(long id)
        {
            var client = GetClient();
            var response = await client.PutAsync($"api/v1/ServiceRequest/{id}/close", null);

            TempData["ToastMessage"] = response.IsSuccessStatusCode ? "Closed successfully" : "Failed to close.";
            TempData["ToastType"] = response.IsSuccessStatusCode ? "success" : "danger";
            return RedirectToAction("List");
        }
    }
}
