using ModelContextProtocol.Server;
using WeatherMcp.Services;
using WeatherMcp.Tools;
using System.ComponentModel;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add weather service
builder.Services.AddSingleton<IWeatherService, WeatherService>();

// Add weather tools for MCP
builder.Services.AddScoped<WeatherTools>();

// Add MCP server with HTTP transport
builder.Services.AddMcpServer()
    .WithHttpTransport()
    .WithToolsFromAssembly();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

var weatherService = app.Services.GetRequiredService<IWeatherService>();

// REST API endpoints
app.MapGet("/weatherforecast", () =>
{
    return weatherService.GetWeatherForecast();
})
.WithName("GetWeatherForecast")
.WithOpenApi();

app.MapGet("/weatherforecast/{city}", (string city) =>
{
    return weatherService.GetWeatherForecast(city);
})
.WithName("GetWeatherForecastForCity")
.WithOpenApi();

app.MapGet("/weatherforecast/date/{startDate}", (string startDate) =>
{
    if (DateOnly.TryParse(startDate, out var date))
    {
        return Results.Ok(weatherService.GetWeatherForecast(date));
    }
    return Results.BadRequest($"Invalid date format: {startDate}. Please use YYYY-MM-DD format.");
})
.WithName("GetWeatherForecastForDate")
.WithOpenApi();

app.MapGet("/weatherforecast/{city}/date/{startDate}", (string city, string startDate) =>
{
    if (DateOnly.TryParse(startDate, out var date))
    {
        return Results.Ok(weatherService.GetWeatherForecast(city, date));
    }
    return Results.BadRequest($"Invalid date format: {startDate}. Please use YYYY-MM-DD format.");
})
.WithName("GetWeatherForecastForCityAndDate")
.WithOpenApi();

// Map MCP endpoints
app.MapMcp();

app.Run();

// Make the Program class public for testing
public partial class Program { }
