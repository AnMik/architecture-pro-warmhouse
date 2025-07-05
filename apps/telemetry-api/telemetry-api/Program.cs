using Microsoft.AspNetCore.Mvc;
using telemetry_api;

var app = WebApplication.CreateBuilder(args).Build();

app.MapGet(
    "/api/v2/sensors/temperature/{location}",
    ([FromRoute] string location) 
        => new Response.Temperature(
            location,
            GetTemperature(), 
            "C", 
            "On", 
            DateTime.Now, 
            "Temperature from NEW SHINY MICROSERVICE system."));

app.MapPatch(
    "/api/v2/sensors/{id}/value",
    ([FromRoute] string id, [FromBody] Request.SensorValue sensorValue) 
        => new Response.Temperature(
            "Kitchen",
            sensorValue.Value, 
            "C", 
            sensorValue.Status, 
            DateTime.Now, 
            "Temperature updated in NEW SHINY MICROSERVICE system."));

app.Run("http://+:8084");
return;

double GetTemperature() => (Random.Shared.NextDouble() - 0.5) * 100;