using HTM.Infrastructure.Devices.Enums;
using HTM.Infrastructure.MessagesBase;

namespace HTM.Infrastructure.Devices.Messages.Events;

public class DeviceConnectionChangedEvent : EventBase
{
    public DeviceType DeviceType { get; }
    public bool IsConnected { get; }

    public DeviceConnectionChangedEvent(DeviceType deviceType, bool isConnected)
    {
        DeviceType = deviceType;
        IsConnected = isConnected;
    }
}