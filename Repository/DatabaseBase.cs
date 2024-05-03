using System.Data.SqlClient;
using System.Reflection.Metadata.Ecma335;
using WeatherByGoogleLocation.Models;
using WeatherByGoogleLocation.Models.Forecast;

namespace WeatherByGoogleLocation.Repository
{
    public class DatabaseBase
    {
        public void SaveToDatabase(LocationForecast? forecastJson)
        {
            if (forecastJson == null)
            {
                // error checking and logging
            }
            else
            {
                try
                {
                    string connectionString = "data source=UmberTower;initial catalog=WeatherByGoogleLocation;trusted_connection=true";

                    // Query to be executed
                    string query = "INSERT INTO dbo.forecast (weatherJSONData) " +
                                       "VALUES (@weatherJSONData) ";

                    // instance connection and command
                    using (SqlConnection cn = new SqlConnection(connectionString))
                    using (SqlCommand cmd = new SqlCommand(query, cn))
                    {
                        // add parameters and their values
                        cmd.Parameters.Add("@weatherJSONData", System.Data.SqlDbType.NVarChar, -1).Value = System.Text.Json.JsonSerializer.Serialize(forecastJson);

                        // open connection, execute command and close connection
                        cn.Open();
                        cmd.ExecuteNonQuery();
                        cn.Close();
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
        }

        public WeatherSettings GetSettings()
        {
            WeatherSettings weatherSettings = new WeatherSettings("",0,0);

            try
            {
                string connectionString = "data source=UmberTower;initial catalog=WeatherByGoogleLocation;trusted_connection=true";

                // Query to be executed
                string query = "SELECT TOP 1 [GoogleMapApiKey],[DefaultLatitude],[DefaultLongitude] FROM [dbo].[Settings];";

                // instance connection and command
                using (SqlConnection cn = new SqlConnection(connectionString))
                using (SqlCommand cmd = new SqlCommand(query, cn))
                {
                    // open connection, execute command and close connection
                    cn.Open();
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read()) // Don't assume we have any rows.
                        {
                            string GoogleMapApiKey = reader.GetString(reader.GetOrdinal("GoogleMapApiKey"));
                            double DefaultLatitude = (double)reader.GetDecimal(reader.GetOrdinal("DefaultLatitude"));
                            double DefaultLongitude = (double)reader.GetDecimal(reader.GetOrdinal("DefaultLongitude"));
                            weatherSettings = new WeatherSettings(GoogleMapApiKey, DefaultLatitude, DefaultLongitude);
                        }
                        else {
                            throw new Exception("No data");
                        }
                    }
                    cn.Close();
                }

                return weatherSettings;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

    }
}
