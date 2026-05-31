using MFO_Logistics_Web_UI.Models;
using MFO_Logistics_Web_UI.Models.DTOs;
using MFO_Logistics_Web_UI.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Net.Http.Headers;
using System.Text.Json.Serialization;

namespace MFO_Logistics_Web_UI.Controllers
{
    public class HomeController : Controller
    {

        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        public HomeController(
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

            var url = $"{baseUrl}/api/Receipt";

            var response = await client.GetAsync(url);


            if (!response.IsSuccessStatusCode)
            {
                ViewBag.Error = "Receipt verileri alýnamadý.";                
                return View();
            }

            var json = await response.Content.ReadAsStringAsync();

            var receipts = await response.Content.ReadFromJsonAsync<List<ReceiptSearchDTO>>();


            var model = new ReceiptDashboardViewModel
            {
                Completed = receipts.Count(x => x.ReceiptStatusName == "Tamamlandý"),
                Waiting = receipts.Count(x => x.ReceiptStatusName == "Beklemede"),
                Cancelled = receipts.Count(x => x.ReceiptStatusName == "Ýptal Edildi"),
                InProgress = receipts.Count(x => x.ReceiptStatusName == "Devam Ediyor")
            };

            return View(model);


        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
