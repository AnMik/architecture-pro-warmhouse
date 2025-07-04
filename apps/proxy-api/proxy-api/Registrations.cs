namespace proxy_api;

internal static class Registrations
{
    internal static IServiceCollection RegisterClients(this IServiceCollection servicesCollection)
    {
        var legacyApiUrl = Environment.GetEnvironmentVariable("TEMPERATURE_API_URL");
        var sensorApiUrl = Environment.GetEnvironmentVariable("SENSOR_API_URL");
        var telemetryApiUrl = Environment.GetEnvironmentVariable("TELEMETRY_API_URL");

        servicesCollection.AddHttpClient(HttpClientNames.LegacyApi, client => client.BaseAddress = new Uri(legacyApiUrl));
        servicesCollection.AddHttpClient(HttpClientNames.SensorApi, client => client.BaseAddress = new Uri(sensorApiUrl));
        servicesCollection.AddHttpClient(HttpClientNames.TelemetryApi, client => client.BaseAddress = new Uri(telemetryApiUrl));
        
        return servicesCollection;
    }
}