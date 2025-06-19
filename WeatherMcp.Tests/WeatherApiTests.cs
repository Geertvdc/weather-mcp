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
}