using WeatherMcp.Services;
using WeatherMcp;

namespace WeatherMcp.Tests;

public class WeatherServiceTests
{
    private readonly IWeatherService _weatherService;

    public WeatherServiceTests()
    {
        _weatherService = new WeatherService();
    }

    [Fact]
    public void GetWeatherForecast_ReturnsDefaultCity_WhenNoCityProvided()
    {
        // Act
        var forecast = _weatherService.GetWeatherForecast();

        // Assert
        Assert.NotNull(forecast);
        Assert.Equal(5, forecast.Length);
        Assert.All(forecast, f => Assert.Equal("Default City", f.City));
    }

    [Fact]
    public void GetWeatherForecast_ReturnsSpecificCity_WhenCityProvided()
    {
        // Arrange
        const string city = "New York";

        // Act
        var forecast = _weatherService.GetWeatherForecast(city);

        // Assert
        Assert.NotNull(forecast);
        Assert.Equal(5, forecast.Length);
        Assert.All(forecast, f => Assert.Equal(city, f.City));
    }

    [Fact]
    public void GetWeatherForecast_ReturnsCorrectNumberOfDays()
    {
        // Act
        var forecast = _weatherService.GetWeatherForecast();

        // Assert
        Assert.Equal(5, forecast.Length);
    }

    [Fact]
    public void GetWeatherForecast_ReturnsValidTemperatureRange()
    {
        // Act
        var forecast = _weatherService.GetWeatherForecast();

        // Assert
        Assert.All(forecast, f =>
        {
            Assert.InRange(f.TemperatureC, -20, 55);
            Assert.InRange(f.TemperatureF, -4, 131); // Celsius to Fahrenheit conversion
        });
    }

    [Fact]
    public void GetWeatherForecast_ReturnsFutureDates()
    {
        // Arrange
        var today = DateOnly.FromDateTime(DateTime.Now);

        // Act
        var forecast = _weatherService.GetWeatherForecast();

        // Assert
        Assert.All(forecast, f => Assert.True(f.Date > today));
    }

    [Fact]
    public void GetWeatherForecast_ReturnsValidSummaries()
    {
        // Arrange
        var validSummaries = new[] { "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching" };

        // Act
        var forecast = _weatherService.GetWeatherForecast();

        // Assert
        Assert.All(forecast, f => Assert.Contains(f.Summary, validSummaries));
    }
}