using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http.Json;
using WeatherMcp;

namespace WeatherMcp.Tests;

public class WeatherApiTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;
    private readonly HttpClient _client;

    public WeatherApiTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
        _client = _factory.CreateClient();
    }

    [Fact]
    public async Task GetWeatherForecast_ReturnsSuccessStatusCode()
    {
        // Act
        var response = await _client.GetAsync("/weatherforecast");

        // Assert
        response.EnsureSuccessStatusCode();
    }

    [Fact]
    public async Task GetWeatherForecast_ReturnsJsonContent()
    {
        // Act
        var response = await _client.GetAsync("/weatherforecast");

        // Assert
        Assert.Equal("application/json; charset=utf-8", response.Content.Headers.ContentType?.ToString());
    }

    [Fact]
    public async Task GetWeatherForecast_ReturnsFiveDayForecast()
    {
        // Act
        var forecast = await _client.GetFromJsonAsync<WeatherForecast[]>("/weatherforecast");

        // Assert
        Assert.NotNull(forecast);
        Assert.Equal(5, forecast.Length);
    }

    [Fact]
    public async Task GetWeatherForecast_ReturnsValidData()
    {
        // Act
        var forecast = await _client.GetFromJsonAsync<WeatherForecast[]>("/weatherforecast");

        // Assert
        Assert.NotNull(forecast);
        Assert.All(forecast, f =>
        {
            Assert.True(f.Date > DateOnly.FromDateTime(DateTime.Now));
            Assert.InRange(f.TemperatureC, -20, 55);
            Assert.NotNull(f.Summary);
            Assert.Equal("Default City", f.City);
        });
    }

    [Theory]
    [InlineData("London")]
    [InlineData("New York")]
    [InlineData("Tokyo")]
    public async Task GetWeatherForecastForCity_ReturnsCorrectCity(string city)
    {
        // Act
        var forecast = await _client.GetFromJsonAsync<WeatherForecast[]>($"/weatherforecast/{city}");

        // Assert
        Assert.NotNull(forecast);
        Assert.Equal(5, forecast.Length);
        Assert.All(forecast, f => Assert.Equal(city, f.City));
    }

    [Fact]
    public async Task GetWeatherForecastForCity_ReturnsSuccessStatusCode()
    {
        // Act
        var response = await _client.GetAsync("/weatherforecast/London");

        // Assert
        response.EnsureSuccessStatusCode();
    }

    [Fact]
    public async Task GetWeatherForecastForDate_ReturnsCorrectStartDate()
    {
        // Act
        var forecast = await _client.GetFromJsonAsync<WeatherForecast[]>("/weatherforecast/date/2024-06-15");

        // Assert
        Assert.NotNull(forecast);
        Assert.Equal(5, forecast.Length);
        Assert.Equal(new DateOnly(2024, 6, 15), forecast[0].Date);
        Assert.Equal(new DateOnly(2024, 6, 19), forecast[4].Date);
        Assert.All(forecast, f => Assert.Equal("Default City", f.City));
    }

    [Fact]
    public async Task GetWeatherForecastForDate_WithInvalidDate_ReturnsBadRequest()
    {
        // Act
        var response = await _client.GetAsync("/weatherforecast/date/invalid-date");

        // Assert
        Assert.Equal(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task GetWeatherForecastForCityAndDate_ReturnsCorrectCityAndDate()
    {
        // Act
        var forecast = await _client.GetFromJsonAsync<WeatherForecast[]>("/weatherforecast/Tokyo/date/2024-12-25");

        // Assert
        Assert.NotNull(forecast);
        Assert.Equal(5, forecast.Length);
        Assert.Equal(new DateOnly(2024, 12, 25), forecast[0].Date);
        Assert.All(forecast, f => Assert.Equal("Tokyo", f.City));
    }

    [Fact]
    public async Task GetWeatherForecastForCityAndDate_WithInvalidDate_ReturnsBadRequest()
    {
        // Act
        var response = await _client.GetAsync("/weatherforecast/Berlin/date/2024-13-45");

        // Assert
        Assert.Equal(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
    }
}