namespace WeatherByGoogleLocation.Models
{
    public class WeatherSettings
    {
        private string _googleMapApiKey;
        private double _defaultLatitude;
        private double _defaultLongitude;

        public WeatherSettings(string googleMapApiKey, double defaultLatitude, double defaultLongitude) {
            _googleMapApiKey = googleMapApiKey;
            _defaultLatitude = defaultLatitude;
            _defaultLongitude = defaultLongitude;   
        }

        public string GoogleMapApiKey { get => _googleMapApiKey; }
        public double DefaultLatitude { get => _defaultLatitude; }
        public double DefaultLongitude { get => _defaultLongitude; }
    }
}
