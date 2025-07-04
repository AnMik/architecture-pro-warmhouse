using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();

app.MapGet(
    "/api/v2/sensors", 
    () => new SensorsResponse([new SensorResponse(Random.Shared.NextDouble(), "SomeStatus", DateTime.Now)]));

app.MapGet(
    "/api/v2/sensors/{id}", 
    (string id) => new SensorResponse(Random.Shared.NextDouble(), $"Status for {id}", DateTime.Now));

app.MapPost(
    "/api/v2/sensors", 
    (HttpRequest httpRequest) => new SensorResponse(Random.Shared.NextDouble(), JsonSerializer.Serialize(httpRequest.Body), DateTime.Now));

app.MapPut(
    "/api/v2/sensors/{id}", 
    (string id) => new SensorResponse(Random.Shared.NextDouble(), $"Status for {id}", DateTime.Now));

app.MapDelete(
    "/api/v2/sensors/{id}",
    (string id) => Random.Shared.NextDouble() >= 0.5
        ? new DefaultResponse(Message: $"{id} deleted")
        : new DefaultResponse(Error: "Deleting error"));

app.Run("http://+:8083");
return;

internal record DefaultResponse(string? Message = null, string? Error = null);

internal record SensorResponse(double Value, string Status, DateTime LastUpdated);

internal record SensorsResponse(SensorResponse[] Sensors);
