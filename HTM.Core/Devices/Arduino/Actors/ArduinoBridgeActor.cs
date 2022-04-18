using Akka.Actor;
using HTM.Infrastructure.Akka;
using HTM.Infrastructure.Devices.Adapters;
using HTM.Infrastructure.Devices.Enums;
using HTM.Infrastructure.Devices.Messages;
using HTM.Infrastructure.Devices.Messages.Events;
using HTM.Infrastructure.Devices.Messages.Requests;
using HTM.Infrastructure.Models;

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
        _serialPortDevice.OnMessagesReceived += NotifyArduinoMessagesReceived;

        Receive<InitializationResultMessage>(
            _ => Logger.Info("ArduinoBridgeActor | Device initialization success"),
            result => !result.IsError);
        
        Receive<InitializationResultMessage>(
            result => Logger.Error("ArduinoBridgeActor | Device initialization Error={Error}", result.Exception.Message),
            result => result.IsError);

        Receive<SendMessageRequest>(SendMessage);
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

    private void NotifyArduinoConnectionChanged(object sender, bool isConnected)
    {
        _deviceActor.Tell(new DeviceConnectionChangedEvent(DeviceType.Arduino, isConnected));
    }

    private void NotifyArduinoMessagesReceived(object sender, SerialPortMessage[] messages)
    {
        foreach (var message in messages)
        {
            Logger.Info("NotifyArduinoMessageReceived | Type={Type}, Text={Text}", message.Type, message.Text);
        }
        
        _deviceActor.Tell(new SerialPortMessageReceivedEvent(messages));
    }
}