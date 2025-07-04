using proxy_api;

var builder = WebApplication.CreateBuilder(args);

builder.Services.RegisterClients();

var app = builder.Build();

app.MapGet("/api/v1/sensors", Handlers.GetSensorsHandler()); // Регистрируем хэндлер по старому пути, внутри хэндлера на основании стратегии миграции выбираем легаси или новую систему
app.MapGet("/api/v1/sensors/{id}", Handlers.GetSensorByIdHandler());
app.MapPost("/api/v1/sensors", Handlers.CreateSensorHandler());
app.MapPut("/api/v1/sensors/{id}", Handlers.UpdateSensorHandler());
app.MapDelete("/api/v1/sensors/{id}", Handlers.DeleteSensorHandler());
app.MapGet("/api/v1/sensors/temperature/{location}", Handlers.GetTemperatureHandler());
app.MapPatch("/api/v1/sensors/{id}/value", Handlers.UpdateTemperatureHandler());

app.Run("http://+:8080");
