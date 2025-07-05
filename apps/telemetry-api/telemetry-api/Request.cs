namespace telemetry_api;

internal abstract class Request
{
    internal record SensorValue(double Value, string Status);
}