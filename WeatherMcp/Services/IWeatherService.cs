using WeatherMcp;

namespace WeatherMcp.Services;

public interface IWeatherService
{
    WeatherForecast[] GetWeatherForecast();
    WeatherForecast[] GetWeatherForecast(string city);
}