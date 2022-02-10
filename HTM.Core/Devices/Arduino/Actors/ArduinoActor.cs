using HTM.Core.Actors;
using HTM.Infrastructure;
using HTM.Infrastructure.Adapters;
using HTM.Infrastructure.Devices.Enums;
using HTM.Infrastructure.Devices.Messages.Events;
using HTM.Infrastructure.Devices.Messages.Requests;
using Akka.Actor;
using Akka.Event;
using System.Runtime.CompilerServices;

namespace HTM.Core.Devices.Arduino.Actors;

record DeviceConnected;

public record TestMessage(string Message);

public class ArduinoActor : BaseActor
{
    private readonly ISerialPortDevice _serialPortDevice;

    private IActorRef _testActor;

    public ArduinoActor(ISerialPortDevice serialPortDevice)
    {
        _serialPortDevice = serialPortDevice;

        _testActor = Context.ActorOf(Props.Create<TestActor>(), nameof(TestActor));
        
        Become(DeviceNotConnectedBehavior);
    }

    private void DeviceNotConnectedBehavior()
    {
        Receive<DeviceConnected>(_ => Become(DeviceConnectedBehavior));
        Receive<InitializeDeviceEvent>(_ => _serialPortDevice.Initialize());
        Context.System.EventStream.Subscribe<InitializeDeviceEvent>(Self);
        
        _serialPortDevice.Initialize()
            .PipeTo(Self, Self, () => new DeviceConnected());
    }
    
    private void DeviceConnectedBehavior()
    {
        _serialPortDevice.ConnectionChanged += (s, m) => OnArduinoConnectionChanged(m);
        _serialPortDevice.OnMessageReceived += (s, m) => OnMessageReceived(m);
        
        Receive<GetTemperatureRequest>(OnGetTemperatureRequest);
        Context.System.EventStream.Subscribe<GetTemperatureRequest>(Self);
    }

    private void OnArduinoConnectionChanged(bool isConnected)
    {
        //Context.System.EventStream.Publish(new DeviceConnectionChangedEvent(DeviceType.SerialPort, isConnected));
    }

    private void OnMessageReceived(string message)
    {
        _testActor.Tell(new TestMessage(message));
        //Sender.Tell(new GetTemperatureResponse(message));
    }

    private void OnGetTemperatureRequest(GetTemperatureRequest request)
    {
        _serialPortDevice.SendMessage(SerialPortCommands.GetTemperature);
    }
}