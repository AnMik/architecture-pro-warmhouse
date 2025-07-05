namespace proxy_api;

internal static class HttpRequestExtensions
{
    public static async Task<string> ReadContent(this HttpRequest httpRequest, CancellationToken ct)
    {
        using var reader = new StreamReader(httpRequest.Body);
        return await reader.ReadToEndAsync(ct);
    }
}

internal static class HttpResponseMessageExtensions
{
    public static Task<string> ReadContent(this HttpResponseMessage httpResponseMessage, CancellationToken ct) 
        => httpResponseMessage.Content.ReadAsStringAsync(ct);
    
    public static string? GetContentType(this HttpResponseMessage httpResponseMessage) 
        => httpResponseMessage.Content.Headers.ContentType?.MediaType;
}