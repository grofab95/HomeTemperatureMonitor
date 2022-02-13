using Akka.Event;
using HTM.Infrastructure.Devices.Messages.Events;

namespace HTM.Core.Actors;

public class DevicesHealthMonitorActor : BaseActor
{
    public DevicesHealthMonitorActor()
    {
        Receive<DeviceConnectionChangedEvent>(LogDeviceConnectionChanged);

        Context.System.EventStream.Subscribe<DeviceConnectionChangedEvent>(Self);
    }

    private void LogDeviceConnectionChanged(DeviceConnectionChangedEvent @event)
    {
        Logger.Info("{DeviceConnectionChangedEvent} | Device={Device}, IsConnected={IsConnected}",
            nameof(DeviceConnectionChangedEvent), @event.DeviceType, @event.IsConnected);
    }
}