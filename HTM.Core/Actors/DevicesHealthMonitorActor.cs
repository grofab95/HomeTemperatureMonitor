using Akka.Actor;
using Akka.Event;
using HTM.Infrastructure.Akka;
using HTM.Infrastructure.Devices.Enums;
using HTM.Infrastructure.Devices.Messages.Events;
using HTM.Infrastructure.Devices.Messages.Requests;

namespace HTM.Core.Actors;

public class DevicesHealthMonitorActor : BaseActor
{
    private readonly Dictionary<DeviceType, bool> _devicesConnectionsState =
        Enum.GetValues(typeof(DeviceType))
            .Cast<DeviceType>()
            .ToDictionary(x => x, x => false);

    public DevicesHealthMonitorActor()
    {
        Receive<DeviceConnectionChangedEvent>(OnDeviceConnectionChanged);
        Receive<GetDeviceConnectionStateRequest>(request =>
        {
            var isConnected = _devicesConnectionsState[request.DeviceType];
            Sender.Tell(new GetDeviceConnectionStateResponse(request.RequestId, isConnected));
        });

        Context.System.EventStream.Subscribe<DeviceConnectionChangedEvent>(Self);
        Context.System.EventStream.Subscribe<GetDeviceConnectionStateRequest>(Self);
    }

    private void OnDeviceConnectionChanged(DeviceConnectionChangedEvent @event)
    {
        Logger.Info("{DeviceConnectionChangedEvent} | Device={Device}, IsConnected={IsConnected}",
            nameof(DeviceConnectionChangedEvent), @event.DeviceType, @event.IsConnected);

        _devicesConnectionsState[@event.DeviceType] = @event.IsConnected;
    }
}