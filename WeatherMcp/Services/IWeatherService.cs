using WeatherMcp;

namespace WeatherMcp.Services;

public interface IWeatherService
{
    WeatherForecast[] GetWeatherForecast();
    WeatherForecast[] GetWeatherForecast(string city);
    WeatherForecast[] GetWeatherForecast(DateOnly startDate);
    WeatherForecast[] GetWeatherForecast(string city, DateOnly startDate);
}