namespace proxy_api;

internal static class MigrationStrategy
{
    public static bool UseMicroservices()
    {
        var redirectPercentFromConfig = Environment.GetEnvironmentVariable("REDIRECT_PERCENT");
    
        var redirectPercent = double.TryParse(redirectPercentFromConfig, out var percent)
            ? Math.Max(Math.Min(percent, 100), 0) / 100
            : 0;
    
        return redirectPercent > Random.Shared.NextDouble();
    }
}