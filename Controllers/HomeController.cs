using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Xml;
using WeatherByGoogleLocation.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WeatherByGoogleLocation.Models.Forecast;
using System.Xml.Linq;


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

        public IActionResult Index(double lat, double lng)
        {
            ViewBag.APIKey = "AIzaSyDJ9csYo9ulNkY8CTUHDVGUTbQ4_8CuwEw";
            var weatherData = GetWeatherData(lat,lng);

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

        public WeatherForecast GetWeatherData(double latitude, double longitude)
        {
            List<WeatherData> forecast = new List<WeatherData>();

            if (latitude == 0) { latitude = 41.591494859495114; }
            if (longitude == 0) { longitude = -93.60379396555436; }

            var weatherURL = $"https://api.weather.gov/points/{latitude},{longitude}";
            var weatherDataJSON = GetAPIAsync(weatherURL).Result;
            var data = JsonConvert.DeserializeObject<WeatherLatLngData>(weatherDataJSON);

            var forecastURL = $"https://api.weather.gov/gridpoints/{data.properties.gridId}/{data.properties.gridX},{data.properties.gridY}/forecast";
            var forecastJson = JsonConvert.DeserializeObject<LocationForecast>(GetAPIAsync(forecastURL).Result);

            var forcastHourlyURL = $"https://api.weather.gov/gridpoints/TOP/32,81/forecast/hourly";
            var forcastHourly = JsonConvert.DeserializeObject<dynamic>(GetAPIAsync(forcastHourlyURL).Result);

            foreach (Period period in forecastJson.properties.periods)
            {
                var probabilityOfPrecipitation = (period.probabilityOfPrecipitation.value == null) ? "Approches 0" : period.probabilityOfPrecipitation.value.ToString();
                var probabilityOfPrecipitationUnits = period.probabilityOfPrecipitation.unitCode.Split(":").Last();

                var relativeHumidity = (period.relativeHumidity.value == null) ? "Approches 0" : period.relativeHumidity.value.ToString();
                var relativeHumidityUnits = period.relativeHumidity.unitCode.Split(":").Last();

                forecast.Add(new WeatherData
                {
                    Latitude = latitude,
                    Longitude = longitude,
                    SequenceId = period.number,
                    Name = period.name,
                    StartTime = period.startTime,
                    IsDayTime = period.isDaytime,
                    Temperature = period.temperature,
                    TemperatureUnit = period.temperatureUnit,
                    ProbabilityOfPercipitation = string.Format("{0} {1}", probabilityOfPrecipitation, probabilityOfPrecipitationUnits),
                    RelativeHumidity = string.Format("{0} {1}", relativeHumidity, relativeHumidityUnits),
                    WindSpeed = period.windSpeed,
                    WindDirection = period.windDirection,
                    IconURL = period.icon,
                    ShortForecast = period.shortForecast,
                    DetailedForecast = period.detailedForecast
                });
            }
            return new WeatherForecast() { Forecast = forecast };
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
