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

    [Fact]
    public void GetWeatherForecastForDate_WithValidDate_ReturnsCorrectForecast()
    {
        // Arrange
        const string dateString = "2024-06-15";

        // Act
        var forecast = _weatherTools.GetWeatherForecastForDate(dateString);

        // Assert
        Assert.NotNull(forecast);
        Assert.Equal(5, forecast.Length);
        Assert.Equal(new DateOnly(2024, 6, 15), forecast[0].Date);
        Assert.All(forecast, f => Assert.Equal("Default City", f.City));
    }

    [Fact]
    public void GetWeatherForecastForDate_WithInvalidDate_ThrowsArgumentException()
    {
        // Arrange
        const string invalidDate = "invalid-date";

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => _weatherTools.GetWeatherForecastForDate(invalidDate));
        Assert.Contains("Invalid date format", exception.Message);
    }

    [Fact]
    public void GetWeatherForecastForCityAndDate_WithValidInputs_ReturnsCorrectForecast()
    {
        // Arrange
        const string city = "Paris";
        const string dateString = "2024-12-25";

        // Act
        var forecast = _weatherTools.GetWeatherForecastForCityAndDate(city, dateString);

        // Assert
        Assert.NotNull(forecast);
        Assert.Equal(5, forecast.Length);
        Assert.Equal(new DateOnly(2024, 12, 25), forecast[0].Date);
        Assert.All(forecast, f => Assert.Equal(city, f.City));
    }

    [Fact]
    public void GetWeatherForecastForCityAndDate_WithInvalidDate_ThrowsArgumentException()
    {
        // Arrange
        const string city = "London";
        const string invalidDate = "2024-13-45";

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => _weatherTools.GetWeatherForecastForCityAndDate(city, invalidDate));
        Assert.Contains("Invalid date format", exception.Message);
    }
}