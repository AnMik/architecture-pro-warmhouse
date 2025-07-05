namespace telemetry_api;

internal abstract class Response
{
    internal record Temperature(string Location, double Value, string Unit, string Status, DateTime Timestamp, string Description);
}