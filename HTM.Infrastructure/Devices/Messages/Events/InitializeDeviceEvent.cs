namespace HTM.Infrastructure.Devices.Messages.Events;

public class InitializeDeviceEvent
{
    public static InitializeDeviceEvent Instance => new();

    private InitializeDeviceEvent()
    { }
}