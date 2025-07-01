using ModelContextProtocol.Server;
using System.ComponentModel;
using WeatherMcp.Services;
using WeatherMcp;

namespace WeatherMcp.Tools;

[McpServerToolType]
public class WeatherTools
{
    private readonly IWeatherService _weatherService;

    public WeatherTools(IWeatherService weatherService)
    {
        _weatherService = weatherService;
    }

    [McpServerTool]
    [Description("Get a 5-day weather forecast for the default city")]
    public WeatherForecast[] GetWeatherForecast()
    {
        return _weatherService.GetWeatherForecast();
    }

    [McpServerTool]
    [Description("Get a 5-day weather forecast for a specific city")]
    public WeatherForecast[] GetWeatherForecastForCity(
        [Description("The name of the city to get weather forecast for")] string city)
    {
        return _weatherService.GetWeatherForecast(city);
    }

    [McpServerTool]
    [Description("Get a 5-day weather forecast for the default city starting from a specific date")]
    public WeatherForecast[] GetWeatherForecastForDate(
        [Description("The start date for the forecast (YYYY-MM-DD format)")] string startDate)
    {
        if (DateOnly.TryParse(startDate, out var date))
        {
            return _weatherService.GetWeatherForecast(date);
        }
        throw new ArgumentException($"Invalid date format: {startDate}. Please use YYYY-MM-DD format.");
    }

    [McpServerTool]
    [Description("Get a 5-day weather forecast for a specific city starting from a specific date")]
    public WeatherForecast[] GetWeatherForecastForCityAndDate(
        [Description("The name of the city to get weather forecast for")] string city,
        [Description("The start date for the forecast (YYYY-MM-DD format)")] string startDate)
    {
        if (DateOnly.TryParse(startDate, out var date))
        {
            return _weatherService.GetWeatherForecast(city, date);
        }
        throw new ArgumentException($"Invalid date format: {startDate}. Please use YYYY-MM-DD format.");
    }
}