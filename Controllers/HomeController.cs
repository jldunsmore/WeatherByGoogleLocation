using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Xml;
using WeatherByGoogleLocation.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WeatherByGoogleLocation.Models.Forecast;


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
            var weatherData = GetWeatherData(0,0);
            return View(weatherData);
        }
        public IActionResult ShowWeather(WeatherData weatherData)
        {
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

        [HttpGet]
        public WeatherData GetWeatherData(double lat, double lng)
        {
            if (lat == 0) { lat = 41.591494859495114; }
            if (lng == 0) { lng = -93.60379396555436; }

            var weatherURL = $"https://api.weather.gov/points/{lat},{lng}";
            var weatherDataJSON = GetAPIAsync(weatherURL).Result;
            var data = JsonConvert.DeserializeObject<WeatherLatLngData>(weatherDataJSON);

            var forecastURL = $"https://api.weather.gov/gridpoints/{data.properties.gridId}/{data.properties.gridX},{data.properties.gridY}/forecast";
            var forecast = JsonConvert.DeserializeObject<LocationForecast>(GetAPIAsync(forecastURL).Result);

            var forcastHourlyURL = $"https://api.weather.gov/gridpoints/TOP/32,81/forecast/hourly";
            var forcastHourly = JsonConvert.DeserializeObject<dynamic>(GetAPIAsync(forcastHourlyURL).Result);

            var weatherData = new WeatherData
            {
                Lat = lat,
                Lng = lng,
                RawWeatherData = forecast.properties.periods[0].detailedForecast

            };
            ShowWeather(weatherData);
            return weatherData;
        }

        static async Task<string> GetAPIAsync(string path)
        {
            var data = "";
            HttpResponseMessage response = await client.GetAsync(path);
            if (response.IsSuccessStatusCode)
            {
                data = await response.Content.ReadAsStringAsync();
            }
            return data;
        }

    }
}
