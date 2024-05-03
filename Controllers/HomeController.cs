using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WeatherByGoogleLocation.Models;
using Newtonsoft.Json;
using WeatherByGoogleLocation.Models.Forecast;
using System.Data.SqlClient;
using System.Text.Json;
using WeatherByGoogleLocation.Repository;

namespace WeatherByGoogleLocation.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private APITools _apiTools = new APITools();

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index(double lat, double lng)
        {
            ViewBag.APIKey = _apiTools.GetGoogleMapApiKey();
            var weatherData = _apiTools.GetWeatherData(lat,lng);

            ViewBag.Latitude = weatherData.Forecast[0].Latitude;
            ViewBag.Longitude = weatherData.Forecast[0].Longitude;
            ViewBag.StartTime = weatherData.Forecast[0].StartTime;
            return View(weatherData);
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
