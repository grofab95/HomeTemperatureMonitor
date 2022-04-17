using Akka.Actor;
using Akka.DependencyInjection;
using Akka.Event;
using HTM.Core.Actors;
using HTM.Infrastructure.Akka;
using HTM.Infrastructure.Devices.Messages.Requests;
using HTM.Infrastructure.MessagesBase;

namespace HTM.Core.Devices.Arduino.Actors;

public class ArduinoActor : BaseActor
{
    public ArduinoActor()
    {
        var arduinoBridgeActor = Context.ActorOf(DependencyResolver.For(Context.System).Props<ArduinoBridgeActor>(), nameof(ArduinoBridgeActor));

        Receive<SendMessageHtmRequest>(arduinoBridgeActor.Forward);
        Receive<EventBase>(Context.System.EventStream.Publish);
        
        Context.System.EventStream.Subscribe<SendMessageHtmRequest>(Self);
    }
}