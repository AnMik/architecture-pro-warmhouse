using System.Net.Mime;
using System.Text;

namespace proxy_api;

internal static class ResultFactory
{
    public static async Task<IResult> From(HttpResponseMessage httpResponseMessage, CancellationToken ct)
    {
        var stringContent = await httpResponseMessage.Content.ReadAsStringAsync(ct);
        var contentType = httpResponseMessage.Content.Headers.ContentType?.MediaType;
    
        return Results.Text(stringContent, contentType);
    }
    
    public static async Task<StringContent> CreateContent(HttpRequest httpRequest, CancellationToken ct)
    {
        using var reader = new StreamReader(httpRequest.Body);
        var content = await reader.ReadToEndAsync(ct);
        return  new StringContent(content, Encoding.UTF8, MediaTypeNames.Application.Json);
    }
}