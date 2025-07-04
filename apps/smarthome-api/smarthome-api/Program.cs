using Microsoft.AspNetCore.Mvc;

var app = WebApplication.CreateBuilder(args).Build();

app.MapGet("/temperature", ([FromQuery] string location) => CreateResponse(location, sensorId: null));

app.MapGet("/temperature/{sensorId}", (string sensorId, [FromQuery] string? location) => CreateResponse(location, sensorId));

app.Run("http://0.0.0.0:8081");

return;

static string GetDefaultSensorId(string location)
    => location switch
    {
        "Living Room" => "1",
        "Bedroom" => "2",
        "Kitchen" => "3",
        _ => "0"
    };

static string GetDefaultLocationId(string sensorId)
    => sensorId switch
    {
        "1" => "Living Room",
        "2" => "Bedroom",
        "3" => "Kitchen",
        _ => "Unknown"
    };

static TemperatureResponse CreateResponse(string? location, string? sensorId)
    => new(
        Value: Random.Shared.Next(-20, 55),
        Unit: "C",
        Timestamp: DateTime.Now,
        Location: location ?? GetDefaultLocationId(sensorId ?? string.Empty),
        Status: "On",
        SensorID: sensorId ?? GetDefaultSensorId(location ?? string.Empty),
        SensorType: "Temperature",
        Description: "Temperature measuring sensor.");

internal record TemperatureResponse(
    float Value, 
    string Unit, 
    DateTime Timestamp, 
    string Location, 
    string Status, 
    string SensorID, 
    string SensorType, 
    string Description);
