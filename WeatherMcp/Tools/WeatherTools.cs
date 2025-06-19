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
}