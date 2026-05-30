using MFO_Logistics_Web_UI.Models.DTOs;
using MFO_Logistics_Web_UI.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Text.Json;

namespace MFO_Logistics_Web_UI.Controllers
{
    public class ReceiptController : Controller
    {

        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        public ReceiptController(
            IHttpClientFactory httpClientFactory,
            IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var model = new ReceiptSearchViewModel
            {
                IsSearched = false
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Index(ReceiptSearchViewModel model)
        {
            var token = HttpContext.Session.GetString("JWToken");

            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Login", "Auth");
            }

            var client = _httpClientFactory.CreateClient();

            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

            var baseUrl = _configuration["ApiSettings:BaseUrl"];

            var queryParams = new List<string>();

            if (model.Filter.CreateDate.HasValue)
                queryParams.Add($"createDate={model.Filter.CreateDate.Value:yyyy-MM-dd}");

            if (model.Filter.CreateDate2.HasValue)
                queryParams.Add($"createDate2={model.Filter.CreateDate2.Value:yyyy-MM-dd}");

            if (!string.IsNullOrWhiteSpace(model.Filter.ReceiptCode))
                queryParams.Add($"receiptCode={Uri.EscapeDataString(model.Filter.ReceiptCode)}");

            if (!string.IsNullOrWhiteSpace(model.Filter.DepositorName))
                queryParams.Add($"depositorName={Uri.EscapeDataString(model.Filter.DepositorName)}");

            if (!string.IsNullOrWhiteSpace(model.Filter.LogisticName))
                queryParams.Add($"logisticName={Uri.EscapeDataString(model.Filter.LogisticName)}");

            var queryString = string.Join("&", queryParams);

            var url = $"{baseUrl}/api/Receipt";

            if (!string.IsNullOrWhiteSpace(queryString))
                url += "?" + queryString;

            var response = await client.GetAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                ViewBag.Error = "Receipt verileri alınamadı.";
                model.Receipts = new();
                model.IsSearched = true;
                return View(model);
            }

            var json = await response.Content.ReadAsStringAsync();

            model.Receipts = JsonSerializer.Deserialize<List<ReceiptSearchDTO>>
            (
                json,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            ) ?? new();

            model.IsSearched = true;

            return View(model);
        }

    }
}
