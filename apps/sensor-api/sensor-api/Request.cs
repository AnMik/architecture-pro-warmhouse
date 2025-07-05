namespace sensor_api;

public abstract class Request
{
    public record CreateSensor(string Name, string Type, string Location, string Unit);

    public record UpdateSensor(
        string Name,
        string Type,
        string Location,
        double Value,
        string Unit,
        string Status);
}