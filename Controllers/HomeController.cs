using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Net.Http.Headers;
using WeatherByGoogleLocation.Models;
using static System.Net.WebRequestMethods;

namespace WeatherByGoogleLocation.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        static HttpClient client = new HttpClient();

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
            client.DefaultRequestHeaders.Add("User-Agent", "(weatherbygooglemap, jldunsmore@gmail.com)");
        }

        public IActionResult Index()
        {
            return View();
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

        [HttpGet]
        public string GetWeatherData(double lat, double lng)
        {
            if (lat == 0) { lat = 41.591494859495114; }
            if (lng == 0) { lng = -93.60379396555436; }

            var weatherURL = $"https://api.weather.gov/points/{lat},{lng}";
            var data = GetAPIAsync(weatherURL).Result;

            return data;
        }

        static async Task<string> GetAPIAsync(string path)
        {
            var data = "";
            HttpResponseMessage response = await client.GetAsync(path);
            //if (response.IsSuccessStatusCode)
            //{
                data = await response.Content.ReadAsStringAsync();
            //}
            return data;
        }

    }
}
