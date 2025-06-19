using WeatherMcp.Tools;
using WeatherMcp.Services;
using WeatherMcp;

namespace WeatherMcp.Tests;

public class WeatherToolsTests
{
    private readonly WeatherTools _weatherTools;
    private readonly IWeatherService _weatherService;

    public WeatherToolsTests()
    {
        _weatherService = new WeatherService();
        _weatherTools = new WeatherTools(_weatherService);
    }

    [Fact]
    public void GetWeatherForecast_ReturnsValidForecast()
    {
        // Act
        var forecast = _weatherTools.GetWeatherForecast();

        // Assert
        Assert.NotNull(forecast);
        Assert.Equal(5, forecast.Length);
        Assert.All(forecast, f =>
        {
            Assert.True(f.Date > DateOnly.FromDateTime(DateTime.Now));
            Assert.InRange(f.TemperatureC, -20, 55);
            Assert.NotNull(f.Summary);
            Assert.Equal("Default City", f.City);
        });
    }

    [Fact]
    public void GetWeatherForecastForCity_ReturnsValidForecastForSpecificCity()
    {
        // Arrange
        const string city = "Paris";

        // Act
        var forecast = _weatherTools.GetWeatherForecastForCity(city);

        // Assert
        Assert.NotNull(forecast);
        Assert.Equal(5, forecast.Length);
        Assert.All(forecast, f =>
        {
            Assert.True(f.Date > DateOnly.FromDateTime(DateTime.Now));
            Assert.InRange(f.TemperatureC, -20, 55);
            Assert.NotNull(f.Summary);
            Assert.Equal(city, f.City);
        });
    }

    [Theory]
    [InlineData("Berlin")]
    [InlineData("Sydney")]
    [InlineData("Toronto")]
    public void GetWeatherForecastForCity_ReturnsCorrectCityForMultipleCities(string city)
    {
        // Act
        var forecast = _weatherTools.GetWeatherForecastForCity(city);

        // Assert
        Assert.NotNull(forecast);
        Assert.All(forecast, f => Assert.Equal(city, f.City));
    }
}