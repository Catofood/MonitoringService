namespace WebApi.Models;

public class DeviceSessionDtoToSend
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public string Version { get; set; }
    public int SessionId { get; set; }
}