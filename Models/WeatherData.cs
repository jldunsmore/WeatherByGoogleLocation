namespace WeatherByGoogleLocation.Models
{
    public class WeatherData
    {
        private double lat;
        private double lng;
        private string JSONWeatherData;

        public double Lat { get => lat; set => lat = value; }

        public double Lng { get => lng; set => lng = value; }

        public string RawWeatherData { get => JSONWeatherData; set => JSONWeatherData = value; }
    }
}
