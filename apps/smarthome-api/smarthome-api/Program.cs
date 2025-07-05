using Microsoft.AspNetCore.Mvc;

var app = WebApplication.CreateBuilder(args).Build();

app.MapGet("/temperature", ([FromQuery] string location) => CreateResponse(location, sensorId: null));

app.MapGet(
    "/temperature/{sensorId}", 
    ([FromRoute] string sensorId, [FromQuery] string? location) => CreateResponse(location, sensorId));

app.Run("http://+:8081");
return;

static string GetDefaultSensorId(string location)
    => location switch
    {
        Rooms.LivingRoom => "1",
        Rooms.Bedroom => "2",
        Rooms.Kitchen => "3",
        _ => "0"
    };

static string GetDefaultLocationId(string sensorId)
    => sensorId switch
    {
        "1" => Rooms.LivingRoom,
        "2" => Rooms.Bedroom,
        "3" => Rooms.Kitchen,
        _ => Rooms.Unknown
    };

static TemperatureResponse CreateResponse(string? location, string? sensorId)
    => new(
        Value: Random.Shared.Next(-20, 55),
        Unit: "C",
        Timestamp: DateTime.Now,
        Location: location ?? GetDefaultLocationId(sensorId ?? string.Empty),
        Status: "On",
        SensorID: sensorId ?? GetDefaultSensorId(location ?? string.Empty),
        SensorType: SensorTypes.Temperature,
        Description: "Temperature from HORRIBLE LEGACY system.");

internal static class Rooms
{
    public const string LivingRoom = "Living Room";
    public const string Bedroom = "Bedroom";
    public const string Kitchen = "Kitchen";
    public const string Unknown = "Unknown";
}

internal static class SensorTypes
{
    public const string Temperature = "Temperature";
}

internal record TemperatureResponse(
    float Value, 
    string Unit, 
    DateTime Timestamp, 
    string Location, 
    string Status, 
    string SensorID, 
    string SensorType, 
    string Description);
