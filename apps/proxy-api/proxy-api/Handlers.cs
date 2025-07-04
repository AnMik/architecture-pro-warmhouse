using Microsoft.AspNetCore.Mvc;

namespace proxy_api;

internal static class Handlers
{
    public static Func<IHttpClientFactory, CancellationToken, Task<IResult>> GetSensorsHandler()
        => async ([FromServices] httpClientFactory, ct) =>
        {
            if (MigrationStrategy.UseMicroservices()) // Выбор использования старого монолита или нового микросервиса на основании стратегии (процент редиректов)
            {
                var client = httpClientFactory.CreateClient(HttpClientNames.SensorApi);
                using var responseMessage = await client.GetAsync("/api/v2/sensors", ct);
                return await ResultFactory.From(responseMessage, ct);
            }
            else
            {
                var client = httpClientFactory.CreateClient(HttpClientNames.LegacyApi);
                using var responseMessage = await client.GetAsync("/api/v1/sensors", ct);
                return await ResultFactory.From(responseMessage, ct);
            }
        };
    
    public static Func<IHttpClientFactory, string, CancellationToken, Task<IResult>> GetSensorByIdHandler()
    {
        return async ([FromServices] httpClientFactory, id, ct) =>
        {
            if (MigrationStrategy.UseMicroservices())
            {
                var client = httpClientFactory.CreateClient(HttpClientNames.SensorApi);
                using var responseMessage = await client.GetAsync($"/api/v2/sensors/{id}", ct);
                return await ResultFactory.From(responseMessage, ct);
            }
            else
            {
                var client = httpClientFactory.CreateClient(HttpClientNames.LegacyApi);
                using var responseMessage = await client.GetAsync($"/api/v1/sensors/{id}", ct);
                return await ResultFactory.From(responseMessage, ct);
            }
        };
    }
    
    public static Func<IHttpClientFactory, HttpRequest, CancellationToken, Task<IResult>> CreateSensorHandler()
    {
        return async ([FromServices] httpClientFactory, request, ct) =>
        {
            using var requestContent = await ResultFactory.CreateContent(request, ct);
        
            if (MigrationStrategy.UseMicroservices())
            {
                var client = httpClientFactory.CreateClient(HttpClientNames.SensorApi);
                using var responseMessage = await client.PostAsync("/api/v2/sensors", requestContent, ct);
                return await ResultFactory.From(responseMessage, ct);
            }
            else
            {
                var client = httpClientFactory.CreateClient(HttpClientNames.LegacyApi);
                using var responseMessage = await client.PostAsync("/api/v1/sensors", requestContent, ct);
                return await ResultFactory.From(responseMessage, ct);
            }
        };
    }
    
    public static Func<IHttpClientFactory, string, HttpRequest, CancellationToken, Task<IResult>> UpdateSensorHandler()
    {
        return async ([FromServices] httpClientFactory, id, request, ct) =>
        {
            using var requestContent = await ResultFactory.CreateContent(request, ct);
        
            if (MigrationStrategy.UseMicroservices())
            {
                var client = httpClientFactory.CreateClient(HttpClientNames.SensorApi);
                using var responseMessage = await client.PutAsync($"/api/v2/sensors/{id}", requestContent, ct);
                return await ResultFactory.From(responseMessage, ct);
            }
            else
            {
                var client = httpClientFactory.CreateClient(HttpClientNames.LegacyApi);
                using var responseMessage = await client.PutAsync($"/api/v1/sensors/{id}", requestContent, ct);
                return await ResultFactory.From(responseMessage, ct);
            }
        };
    }
    
    public static Func<IHttpClientFactory, string, CancellationToken, Task<IResult>> DeleteSensorHandler()
    {
        return async ([FromServices] httpClientFactory, id, ct) =>
        {
            if (MigrationStrategy.UseMicroservices())
            {
                var client = httpClientFactory.CreateClient(HttpClientNames.SensorApi);
                using var responseMessage = await client.DeleteAsync($"/api/v2/sensors/{id}", ct);
                return await ResultFactory.From(responseMessage, ct);
            }
            else
            {
                var client = httpClientFactory.CreateClient(HttpClientNames.LegacyApi);
                using var responseMessage = await client.DeleteAsync($"/api/v1/sensors/{id}", ct);
                return await ResultFactory.From(responseMessage, ct);
            }
        };
    }
    
    public static Func<IHttpClientFactory, string, CancellationToken, Task<IResult>> GetTemperatureHandler()
    {
        return async ([FromServices] httpClientFactory, location, ct) =>
        {
            if (MigrationStrategy.UseMicroservices())
            {
                var client = httpClientFactory.CreateClient(HttpClientNames.TelemetryApi);
                using var responseMessage = await client.GetAsync($"/api/v2/sensors/temperature/{location}", ct);
                return await ResultFactory.From(responseMessage, ct);
            }
            else
            {
                var client = httpClientFactory.CreateClient(HttpClientNames.LegacyApi);
                using var responseMessage = await client.GetAsync($"/api/v1/sensors/temperature/{location}", ct);
                return await ResultFactory.From(responseMessage, ct);
            }
        };
    }
    
    public static Func<IHttpClientFactory, string, HttpRequest, CancellationToken, Task<IResult>> UpdateTemperatureHandler()
    {
        return async ([FromServices] httpClientFactory, id, request, ct) =>
        {
            using var requestContent = await ResultFactory.CreateContent(request, ct);
        
            if (MigrationStrategy.UseMicroservices())
            {
                var client = httpClientFactory.CreateClient(HttpClientNames.TelemetryApi);
                using var responseMessage = await client.PatchAsync($"/api/v2/sensors/{id}/value", requestContent, ct);
                return await ResultFactory.From(responseMessage, ct);
            }
            else
            {
                var client = httpClientFactory.CreateClient(HttpClientNames.LegacyApi);
                using var responseMessage = await client.PatchAsync($"/api/v1/sensors/{id}/value", requestContent, ct);
                return await ResultFactory.From(responseMessage, ct);
            }
        };
    }
}