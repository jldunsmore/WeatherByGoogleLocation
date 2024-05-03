using Newtonsoft.Json;
using WeatherByGoogleLocation.Models.Forecast;
using WeatherByGoogleLocation.Models;

namespace WeatherByGoogleLocation.Repository
{
    public class APITools
    {
        static HttpClient client = new HttpClient();
        static DatabaseBase _database = new DatabaseBase();
        private WeatherSettings _weatherSettings;
        private double _latitude;
        private double _longitude;

        public APITools() {
            client.DefaultRequestHeaders.Add("User-Agent", "(weatherbygooglemap, jldunsmore@gmail.com)");

            _weatherSettings = _database.GetSettings();

            _latitude = _weatherSettings.DefaultLatitude;
            _longitude = _weatherSettings.DefaultLongitude;
        }

        public string GetGoogleMapApiKey()
        {
            return _weatherSettings.GoogleMapApiKey;
        }

        public WeatherForecast GetWeatherData(double latitude, double longitude)
        {
            List<WeatherData> forecast = new List<WeatherData>();

            if (latitude == 0) { latitude = _latitude; }
            if (longitude == 0) { longitude = _longitude; }

            var weatherURL = $"https://api.weather.gov/points/{latitude},{longitude}";
            var weatherDataJSON = GetAPIAsync(weatherURL).Result;
            var data = JsonConvert.DeserializeObject<WeatherLatLngData>(weatherDataJSON);

            var forecastURL = $"https://api.weather.gov/gridpoints/{data.properties.gridId}/{data.properties.gridX},{data.properties.gridY}/forecast";
            var forecastJson = JsonConvert.DeserializeObject<LocationForecast>(GetAPIAsync(forecastURL).Result);
            _database.SaveToDatabase(forecastJson);

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
