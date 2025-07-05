namespace sensor_api;

public static class SensorIdFactory
{
    private const int MinValue = 4;

    private static int _current = MinValue - 1;

    public static string GetNext() => $"{Interlocked.Increment(ref _current)}";

    public static IEnumerable<string> GetAllIds() 
        => Enumerable.Range(MinValue, _current - MinValue + 1).Select(x => x.ToString());
}