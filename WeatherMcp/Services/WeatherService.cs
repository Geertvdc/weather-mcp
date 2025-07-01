using WeatherMcp;

namespace WeatherMcp.Services;

public class WeatherService : IWeatherService
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    public WeatherForecast[] GetWeatherForecast()
    {
        return GetWeatherForecast("Default City");
    }

    public WeatherForecast[] GetWeatherForecast(string city)
    {
        var startDate = DateOnly.FromDateTime(DateTime.Now.AddDays(1));
        return GetWeatherForecast(city, startDate);
    }

    public WeatherForecast[] GetWeatherForecast(DateOnly startDate)
    {
        return GetWeatherForecast("Default City", startDate);
    }

    public WeatherForecast[] GetWeatherForecast(string city, DateOnly startDate)
    {
        var forecast = Enumerable.Range(0, 5).Select(index =>
            new WeatherForecast(
                startDate.AddDays(index),
                Random.Shared.Next(-20, 55),
                Summaries[Random.Shared.Next(Summaries.Length)],
                city
            ))
            .ToArray();
        return forecast;
    }
}