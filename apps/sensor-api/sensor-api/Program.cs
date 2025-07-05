using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using sensor_api;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMemoryCache();
builder.Services.AddHttpClient(HttpClientNames.Telemetry, x => x.BaseAddress = new Uri(Environment.GetEnvironmentVariable("TELEMETRY_API_URL") ?? "http://localhost:5000"));
builder.Services.AddHostedService<UpdateSensorsValuesBackgroundService>();

var app = builder.Build();

app.MapGet(
    "/api/v2/sensors",
    ([FromServices] IMemoryCache memoryCache)
        => SensorIdFactory.GetAllIds().Select(id => Map(GetSensorFromDb(memoryCache, id))).ToArray());

app.MapGet(
    "/api/v2/sensors/{id}",
    ([FromServices] IMemoryCache memoryCache, [FromRoute] string id) => Map(GetSensorFromDb(memoryCache, id)));

app.MapPost(
    "/api/v2/sensors",
    ([FromServices] IMemoryCache memoryCache, [FromBody] Request.CreateSensor request) =>
    {
        var newId = SensorIdFactory.GetNext();
        
        var newSensor = new SensorAggregate(
            newId, request.Name, request.Type, request.Location, 0, request.Unit, "Off", DateTime.Now, DateTime.Now);
        
        memoryCache.Set(newSensor.Id, newSensor);

        return Map(memoryCache.Get<SensorAggregate>(newSensor.Id)!);
    });

app.MapPut(
    "/api/v2/sensors/{id}",
    ([FromServices] IMemoryCache memoryCache, [FromRoute] string id, [FromBody] Request.UpdateSensor request) =>
    {
        var sensor = GetSensorFromDb(memoryCache, id);
        
        sensor.Name = request.Name;
        sensor.SensorType = request.Type;
        sensor.Location = request.Location;
        sensor.Value = request.Value;
        sensor.Unit = request.Unit;
        sensor.Status = request.Status;
        sensor.LastUpdated = DateTime.Now;
        
        return Map(sensor);
    });

app.MapDelete(
    "/api/v2/sensors/{id}",
    ([FromServices] IMemoryCache memoryCache, [FromRoute] string id) =>
    {
        if (Random.Shared.NextDouble() < 0.03)
        {
            return new Response.Default(Error: "Oops! Something went wrong, please try again.");
        }

        memoryCache.Remove(id);
        return new Response.Default(Message: $"Sensor {id} deleted from NEW SHINY microservice");
    });

Console.WriteLine("Run");

app.Run("http://+:8083");
return;

Response.Sensor Map(SensorAggregate sensor) =>
    new(sensor.Id, sensor.Name, sensor.SensorType, sensor.Location, sensor.Value, sensor.Unit, sensor.Status, sensor.LastUpdated, sensor.CreatedAt);

InvalidOperationException SensorNotFoundException() => new("Sensor not found");

SensorAggregate GetSensorFromDb(IMemoryCache memoryCache, string id) => 
    memoryCache.Get<SensorAggregate>(id) ?? throw SensorNotFoundException();