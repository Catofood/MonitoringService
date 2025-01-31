namespace WebApi.Models
{
    public class DeviceSessionDtoReceived
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string Version { get; set; }
        public DeviceSessionDtoReceived(Guid id, string name, DateTime startTime, DateTime endTime, string version)
        {
            Id = id;
            Name = name;
            StartTime = startTime;
            EndTime = endTime;
            Version = version;
        }
    }
}