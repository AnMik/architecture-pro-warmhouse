namespace sensor_api;

public class SensorAggregate(
    string id,
    string name,
    string sensorType,
    string location,
    double value,
    string unit,
    string status,
    DateTime lastUpdated,
    DateTime createdAt)
{
    public string Id { get; } = id;
    public string Name { get; set; } = name;
    public string SensorType { get; set; } = sensorType;
    public string Location { get; set; } = location;
    public double Value { get; set; } = value;
    public string Unit { get; set; } = unit;
    public string Status { get; set; } = status;
    public DateTime LastUpdated { get; set; } = lastUpdated;
    public DateTime CreatedAt { get; } = createdAt;
}