using System.Net.Mime;
using System.Text;
using Microsoft.AspNetCore.Mvc;

namespace proxy_api;

internal static class Handlers
{
    public static Func<IHttpClientFactory, CancellationToken, Task<IResult>> GetSensorsHandler()
        => async ([FromServices] httpClientFactory, ct) =>
        {
            if (MigrationStrategy.UseMicroservices())
            {
                var sensorApiClient = httpClientFactory.CreateClient(HttpClientNames.SensorApi);
                using var responseMessage = await sensorApiClient.GetAsync("/api/v2/sensors", ct);
                return await CreateTextResult(responseMessage, ct);
            }
            else
            {
                var legacyApiClient = httpClientFactory.CreateClient(HttpClientNames.LegacyApi);
                using var responseMessage = await legacyApiClient.GetAsync("/api/v1/sensors", ct);
                return await CreateTextResult(responseMessage, ct);
            }
        };

    public static Func<IHttpClientFactory, string, CancellationToken, Task<IResult>> GetSensorByIdHandler()
        => async ([FromServices] httpClientFactory, id, ct) =>
        {
            if (MigrationStrategy.UseMicroservices(id))
            {
                var sensorApiClient = httpClientFactory.CreateClient(HttpClientNames.SensorApi);
                using var responseMessage = await sensorApiClient.GetAsync($"/api/v2/sensors/{id}", ct);
                return await CreateTextResult(responseMessage, ct);
            }
            else
            {
                var legacyApiClient = httpClientFactory.CreateClient(HttpClientNames.LegacyApi);
                using var responseMessage = await legacyApiClient.GetAsync($"/api/v1/sensors/{id}", ct);
                return await CreateTextResult(responseMessage, ct);
            }
        };

    public static Func<IHttpClientFactory, HttpRequest, CancellationToken, Task<IResult>> CreateSensorHandler() => 
        async ([FromServices] httpClientFactory, request, ct) =>
        {
            if (MigrationStrategy.UseMicroservices())
            {
                var client = httpClientFactory.CreateClient(HttpClientNames.SensorApi);
                using var requestContent = await GetContent(request, ct);
                using var responseMessage = await client.PostAsync("/api/v2/sensors", requestContent, ct);
                return await CreateTextResult(responseMessage, ct);
            }
            else
            {
                var legacyApiClient = httpClientFactory.CreateClient(HttpClientNames.LegacyApi);
                using var requestContent = await GetContent(request, ct);
                using var responseMessage = await legacyApiClient.PostAsync("/api/v1/sensors", requestContent, ct);
                return await CreateTextResult(responseMessage, ct);
            }
        };

    public static Func<IHttpClientFactory, string, HttpRequest, CancellationToken, Task<IResult>> UpdateSensorHandler() => 
        async ([FromServices] httpClientFactory, id, request, ct) =>
        {
            if (MigrationStrategy.UseMicroservices(id))
            {
                var sensorApiClient = httpClientFactory.CreateClient(HttpClientNames.SensorApi);
                using var requestContent = await GetContent(request, ct);
                using var responseMessage = await sensorApiClient.PutAsync($"/api/v2/sensors/{id}", requestContent, ct);
                return await CreateTextResult(responseMessage, ct);
            }
            else
            {
                var legacyApiClient = httpClientFactory.CreateClient(HttpClientNames.LegacyApi);
                using var requestContent = await GetContent(request, ct);
                using var responseMessage = await legacyApiClient.PutAsync($"/api/v1/sensors/{id}", requestContent, ct);
                return await CreateTextResult(responseMessage, ct);
            }
        };

    public static Func<IHttpClientFactory, string, CancellationToken, Task<IResult>> DeleteSensorHandler() =>
        async ([FromServices] httpClientFactory, id, ct) =>
        {
            if (MigrationStrategy.UseMicroservices(id))
            {
                var sensorApiClient = httpClientFactory.CreateClient(HttpClientNames.SensorApi);
                using var responseMessage = await sensorApiClient.DeleteAsync($"/api/v2/sensors/{id}", ct);
                return await CreateTextResult(responseMessage, ct);
            }
            else
            {
                var legacyApiClient = httpClientFactory.CreateClient(HttpClientNames.LegacyApi);
                using var responseMessage = await legacyApiClient.DeleteAsync($"/api/v1/sensors/{id}", ct);
                return await CreateTextResult(responseMessage, ct);
            }
        };

    public static Func<IHttpClientFactory, string, CancellationToken, Task<IResult>> GetTemperatureHandler() =>
        async ([FromServices] httpClientFactory, location, ct) =>
        {
            if (MigrationStrategy.UseMicroservices())
            {
                var telemetryApiClient = httpClientFactory.CreateClient(HttpClientNames.TelemetryApi);
                using var responseMessage = await telemetryApiClient.GetAsync($"/api/v2/sensors/temperature/{location}", ct);
                return await CreateTextResult(responseMessage, ct);
            }
            else
            {
                var legacyApiClient = httpClientFactory.CreateClient(HttpClientNames.LegacyApi);
                using var responseMessage = await legacyApiClient.GetAsync($"/api/v1/sensors/temperature/{location}", ct);
                return await CreateTextResult(responseMessage, ct);
            }
        };

    public static Func<IHttpClientFactory, string, HttpRequest, CancellationToken, Task<IResult>> UpdateTemperatureHandler() =>
        async ([FromServices] httpClientFactory, id, request, ct) =>
        {
            if (MigrationStrategy.UseMicroservices(id))
            {
                var telemetryApiClient = httpClientFactory.CreateClient(HttpClientNames.TelemetryApi);
                using var requestContent = await GetContent(request, ct);
                using var responseMessage = await telemetryApiClient.PatchAsync($"/api/v2/sensors/{id}/value", requestContent, ct);
                return await CreateTextResult(responseMessage, ct);
            }
            else
            {
                var legacyApiClient = httpClientFactory.CreateClient(HttpClientNames.LegacyApi);
                using var requestContent = await GetContent(request, ct);
                using var responseMessage = await legacyApiClient.PatchAsync($"/api/v1/sensors/{id}/value", requestContent, ct);
                return await CreateTextResult(responseMessage, ct);
            }
        };

    private static async Task<IResult> CreateTextResult(HttpResponseMessage responseMessage, CancellationToken ct) 
        => Results.Text(await responseMessage.ReadContent(ct), responseMessage.GetContentType());

    private static async Task<StringContent> GetContent(HttpRequest request, CancellationToken ct) 
        => new(await request.ReadContent(ct), Encoding.UTF8, MediaTypeNames.Application.Json);
}