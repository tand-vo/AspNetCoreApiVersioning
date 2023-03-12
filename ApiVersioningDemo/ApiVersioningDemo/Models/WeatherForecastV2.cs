using System;
namespace ApiVersioningDemo.Models.V2;

public class WeatherForecast
{
    /// <summary>
    /// This is a new property for version 2
    /// </summary>
    public Guid Id { get; set; }

    public DateOnly Date { get; set; }

    public int TemperatureC { get; set; }

    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

    public string? Summary { get; set; }
}
