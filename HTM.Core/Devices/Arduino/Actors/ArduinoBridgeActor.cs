using Akka.Actor;
using Akka.Dispatch;
using HTM.Core.Actors;
using HTM.Core.Devices.Arduino.Messages.Events;
using HTM.Core.Devices.Arduino.Messages.Requests;
using HTM.Infrastructure.Devices.Adapters;
using HTM.Infrastructure.Devices.Enums;
using HTM.Infrastructure.Devices.Messages.Events;

namespace HTM.Core.Devices.Arduino.Actors;

public class ArduinoBridgeActor : BaseActor
{
    private readonly ISerialPortDevice _serialPortDevice;
    private readonly IActorRef _deviceActor;

    public ArduinoBridgeActor(ISerialPortDevice serialPortDevice)
    {
        _deviceActor = Context.Parent;
        
        _serialPortDevice = serialPortDevice;
        _serialPortDevice.ConnectionChanged += NotifyArduinoConnectionChanged;
        _serialPortDevice.OnMessageReceived += NotifyArduinoMessageReceived;

        Receive<SendMessageRequest>(SendMessage);
    }

    protected override void PreStart()
    {
        ActorTaskScheduler.RunTask(_serialPortDevice.Initialize);
    }

    private void SendMessage(SendMessageRequest request)
    {
        SendMessageResponse response;
        
        try
        {
            _serialPortDevice.SendMessage(request.Message);

            response = new SendMessageResponse(request.RequestId);
        }
        catch (Exception ex)
        {
            response = new SendMessageResponse(request.RequestId, ex);
        }
        
        Sender.Tell(response);
    }

    private void NotifyArduinoConnectionChanged(object? sender, bool isConnected)
    {
        _deviceActor.Tell(new DeviceConnectionChangedEvent(DeviceType.Arduino, isConnected));
    }

    private void NotifyArduinoMessageReceived(object? sender, string message)
    {
        _deviceActor.Tell(new MessageReceivedEvent(message));
    }
}