using HTM.Infrastructure.Devices.Enums;

namespace HTM.Infrastructure.Devices.Messages.Events;

public class DeviceConnectionChangedEvent
{
    public DeviceType DeviceType { get; }
    public bool IsConnected { get; }

    public DeviceConnectionChangedEvent(DeviceType deviceType, bool isConnected)
    {
        DeviceType = deviceType;
        IsConnected = isConnected;
    }
}