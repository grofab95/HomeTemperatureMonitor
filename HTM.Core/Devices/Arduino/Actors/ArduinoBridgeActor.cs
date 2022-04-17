using Akka.Actor;
using HTM.Infrastructure.Akka;
using HTM.Infrastructure.Devices.Adapters;
using HTM.Infrastructure.Devices.Enums;
using HTM.Infrastructure.Devices.Messages;
using HTM.Infrastructure.Devices.Messages.Events;
using HTM.Infrastructure.Devices.Messages.Requests;

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

        Receive<InitializationResultMessage>(
            _ => Logger.Info("ArduinoBridgeActor | Device initialization success"),
            result => !result.IsError);
        
        Receive<InitializationResultMessage>(
            result => Logger.Error("ArduinoBridgeActor | Device initialization Error={Error}", result.Exception.Message),
            result => result.IsError);

        Receive<SendMessageHtmRequest>(SendMessage);
    }

    protected override void PreStart()
    {
        _serialPortDevice.Initialize()
            .PipeTo(
                Self,
                Self,
                () => InitializationResultMessage.WithSuccess,
                InitializationResultMessage.WithError);
    }

    protected override void PostStop()
    {
        _serialPortDevice?.Dispose();
    }

    private void SendMessage(SendMessageHtmRequest htmRequest)
    {
        SendMessageHtmResponse htmResponse;
        
        try
        {
            _serialPortDevice.SendMessage(htmRequest.Message);

            htmResponse = new SendMessageHtmResponse(htmRequest.RequestId);
        }
        catch (Exception ex)
        {
            htmResponse = new SendMessageHtmResponse(htmRequest.RequestId, ex);
        }
        
        Sender.Tell(htmResponse);
    }

    private void NotifyArduinoConnectionChanged(object sender, bool isConnected)
    {
        _deviceActor.Tell(new DeviceConnectionChangedEvent(DeviceType.Arduino, isConnected));
    }

    private void NotifyArduinoMessageReceived(object sender, string message)
    {
        _deviceActor.Tell(new MessageReceivedEvent(message));
    }
}