using Newtonsoft.Json;

namespace WeatherByGoogleLocation.Models.Forecast
{
    public class LocationForecast
    {
        [JsonProperty("@context")]
        public List<object> context { get; } = new List<object>();
        public string type { get; set; }
        public Geometry geometry { get; set; }
        public Properties properties { get; set; }
    }

    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class Dewpoint
    {
        public string unitCode { get; set; }
        public double value { get; set; }
    }

    public class Elevation
    {
        public string unitCode { get; set; }
        public double value { get; set; }
    }

    public class Geometry
    {
        public string type { get; set; }
        public List<List<List<double>>> coordinates { get; } = new List<List<List<double>>>();
    }

    public class Period
    {
        public int number { get; set; }
        public string name { get; set; }
        public DateTime startTime { get; set; }
        public DateTime endTime { get; set; }
        public bool isDaytime { get; set; }
        public int temperature { get; set; }
        public string temperatureUnit { get; set; }
        public object temperatureTrend { get; set; }
        public ProbabilityOfPrecipitation probabilityOfPrecipitation { get; set; }
        public Dewpoint dewpoint { get; set; }
        public RelativeHumidity relativeHumidity { get; set; }
        public string windSpeed { get; set; }
        public string windDirection { get; set; }
        public string icon { get; set; }
        public string shortForecast { get; set; }
        public string detailedForecast { get; set; }
    }

    public class ProbabilityOfPrecipitation
    {
        public string unitCode { get; set; }
        public int? value { get; set; }
    }

    public class Properties
    {
        public DateTime updated { get; set; }
        public string units { get; set; }
        public string forecastGenerator { get; set; }
        public DateTime generatedAt { get; set; }
        public DateTime updateTime { get; set; }
        public string validTimes { get; set; }
        public Elevation elevation { get; set; }
        public List<Period> periods { get; } = new List<Period>();
    }

    public class RelativeHumidity
    {
        public string unitCode { get; set; }
        public int value { get; set; }
    }

}

