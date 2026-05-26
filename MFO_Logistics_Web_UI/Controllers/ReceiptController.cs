using MFO_Logistics_Web_UI.Models;
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

        public async Task<IActionResult> Index()
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

            var response =
                await client.GetAsync($"{baseUrl}/api/Reports/Receipt");

            if (!response.IsSuccessStatusCode)
            {
                return View(new List<ReceiptReport>());
            }

            var json = await response.Content.ReadAsStringAsync();

            var data = JsonSerializer.Deserialize<List<ReceiptReport>>
            (
                json,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );

            return View(data);
        }

    }
}
