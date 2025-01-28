namespace WebApi.Models;

public class DeviceSession
{
    public required Guid Id { get; set; }
    public required string Name { get; set; }
    public required DateTime StartTime { get; set; }
    public required DateTime EndTime { get; set; }
    public required string Version { get; set; }
}