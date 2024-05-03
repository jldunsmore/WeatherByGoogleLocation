namespace WeatherByGoogleLocation.Models
{
    public class WeatherForecast
    {
        private List<WeatherData> _forecast;
        //private WeatherData12hr _forecast12Hr;

        public List<WeatherData> Forecast { get => _forecast; set => _forecast = value; }

    }
}
