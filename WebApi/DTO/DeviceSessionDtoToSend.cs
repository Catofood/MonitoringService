namespace WebApi.Models;

public class DeviceSessionDtoToSend : DeviceSessionDtoReceived
{
    public int SessionId { get; set; }

    public DeviceSessionDtoToSend(DeviceSessionDtoReceived dto, int sessionId) 
        : base(dto.Id, dto.Name, dto.StartTime, dto.EndTime, dto.Version)
    {
        SessionId = sessionId;
    }
}