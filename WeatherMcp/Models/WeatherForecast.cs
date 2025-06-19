namespace WeatherMcp;

public record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary, string City = "Default City")
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}