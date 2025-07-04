using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();

app.MapGet(
    "/api/v2/sensors/temperature/{location}",
    (string location) 
        => new TemperatureResponse(
            location, 
            Random.Shared.NextDouble(), 
            "C", 
            "SomeStatus", 
            DateTime.Now, 
            "SomeDescription"));

app.MapPatch(
    "/api/v2/sensors/{id}/value",
    (string id, SensorValue sensorValue) => Random.Shared.NextDouble() >= 0.5
        ? new DefaultResponse(Message: $"{id} patched with {JsonSerializer.Serialize(sensorValue)}")
        : new DefaultResponse(Error: "Patching error"));

app.Run("http://+:8084");
return;

internal record SensorValue(double Value, string Status);

internal record DefaultResponse(string? Message = null, string? Error = null);

internal record TemperatureResponse(string Location, double Value, string Unit, string Status, DateTime Timestamp, string Description);
