namespace WeatherByGoogleLocation.Models
{
    public class WeatherData
    {
        private double _latitude;
        private double _longitude;
        private int _sequenceId;
        private string _name;
        private DateTime _startTime;
        private bool _isDayTime;
        private int _temperature;
        private string _probabilityOfPercipitation;
        private string _relativeHumidity;
        private string _temperatureUnit;
        private string _windSpeed;
        private string _windDirection;
        private string _iconURL;
        private string _shortForecast;
        private string _detailedForecast;
     
        public double Latitude { get => _latitude; set => _latitude = value; }

        public double Longitude { get => _longitude; set => _longitude = value; }
        
        public int SequenceId { get => _sequenceId; set => _sequenceId = value; }

        public string Name { get => _name; set => _name = value; }

        public DateTime StartTime { get => _startTime; set => _startTime = value; } 

        public bool IsDayTime { get => _isDayTime && _temperature > 0; set => _isDayTime = value; }

        public int Temperature { get => _temperature; set => _temperature = value; }

        public string ProbabilityOfPercipitation { get => _probabilityOfPercipitation; set => _probabilityOfPercipitation = value; }

        public string RelativeHumidity { get => _relativeHumidity; set => _relativeHumidity = value; }

        public string TemperatureUnit { get => _temperatureUnit; set => _temperatureUnit = value; }

        public string WindSpeed { get => _windSpeed; set => _windSpeed = value; }

        public string WindDirection { get => _windDirection; set => _windDirection = value; }

        public string IconURL { get => _iconURL; set => _iconURL = value; }

        public string ShortForecast { get => _shortForecast; set => _shortForecast = value; }

        public string DetailedForecast { get => _detailedForecast; set => _detailedForecast = value; }

    }
}
