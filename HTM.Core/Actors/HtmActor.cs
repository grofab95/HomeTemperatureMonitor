using Akka.DependencyInjection;
using HTM.Communication.Actors;
using HTM.Core.Devices.Arduino.Actors;
using HTM.Infrastructure.Akka;

namespace HTM.Core.Actors;

public class HtmActor : BaseActor
{
    public HtmActor()
    {
        Context.ActorOf(DependencyResolver.For(Context.System).Props<ArduinoActor>(), nameof(ArduinoActor));
        Context.ActorOf(DependencyResolver.For(Context.System).Props<ArduinoMessengerActor>(), nameof(ArduinoMessengerActor));
        Context.ActorOf(DependencyResolver.For(Context.System).Props<DevicesHealthMonitorActor>(), nameof(DevicesHealthMonitorActor));
        //Context.ActorOf(DependencyResolver.For(Context.System).Props<TemperatureMonitorActor>(), nameof(TemperatureMonitorActor));
        Context.ActorOf(DependencyResolver.For(Context.System).Props<CommunicationActor>(), nameof(CommunicationActor));
    }
}