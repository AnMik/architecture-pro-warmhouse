namespace sensor_api;

public abstract class Response
{
    internal record Default(string? Message = null, string? Error = null);

    internal record Sensor(
        string Id,
        string Name,
        string SensorType,
        string Location,
        double Value,
        string Unit,
        string Status,
        DateTime LastUpdated,
        DateTime CreatedAt);
    
    internal record Temperature(string Location, double Value, string Unit, string Status, DateTime Timestamp, string Description);
}