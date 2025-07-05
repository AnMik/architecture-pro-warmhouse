using Microsoft.Extensions.Caching.Memory;

namespace sensor_api;

/// <summary>
/// Фоновая задача обновления текущих значений датчиков.
/// </summary>
public class UpdateSensorsValuesBackgroundService(IHttpClientFactory httpClientFactory, IMemoryCache memoryCache)
    : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken ct)
    {
        var telemetryApi = httpClientFactory.CreateClient(HttpClientNames.Telemetry);

        while (!ct.IsCancellationRequested)
        {
            await Task.Delay(TimeSpan.FromMinutes(1), ct);
            
            var sensors = SensorIdFactory.GetAllIds().Select(memoryCache.Get<SensorAggregate>).Where(x => x != null);

            foreach (var sensor in sensors)
            {
                var response = await telemetryApi.GetFromJsonAsync<Response.Temperature>($"/api/v2/sensors/temperature/{sensor.Location}", ct);

                sensor.Value = response.Value;
                sensor.Status = response.Status;
                sensor.Unit = response.Unit;
                sensor.LastUpdated = response.Timestamp;
            }
        }
    }
}