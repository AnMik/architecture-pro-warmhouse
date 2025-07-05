using proxy_api;

var builder = WebApplication.CreateBuilder(args);
builder.Services.RegisterHttpClients();
var app = builder.Build();

// Регистрируем хэндлеры по старым путям, внутри хэндлеров на основании стратегии миграции выбираем между легаси или новой системой
app.MapGet("/api/v1/sensors", Handlers.GetSensorsHandler());
app.MapGet("/api/v1/sensors/{id}", Handlers.GetSensorByIdHandler());
app.MapPost("/api/v1/sensors", Handlers.CreateSensorHandler());
app.MapPut("/api/v1/sensors/{id}", Handlers.UpdateSensorHandler());
app.MapDelete("/api/v1/sensors/{id}", Handlers.DeleteSensorHandler());
app.MapGet("/api/v1/sensors/temperature/{location}", Handlers.GetTemperatureHandler());
app.MapPatch("/api/v1/sensors/{id}/value", Handlers.UpdateTemperatureHandler());
app.MapGet("/health", () => Results.Ok());

app.Run("http://+:8080");
