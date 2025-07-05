namespace proxy_api;

internal static class MigrationStrategy
{
    /// <summary>
    /// Smart migration algorithm based on sensor id
    /// </summary>
    public static bool UseMicroservices(string sensorId) => int.TryParse(sensorId, out var value) && value > 3;
    
    /// <summary>
    /// Smart migration algorithm based on random
    /// </summary>
    public static bool UseMicroservices() => Random.Shared.NextDouble() > 0.5;
}