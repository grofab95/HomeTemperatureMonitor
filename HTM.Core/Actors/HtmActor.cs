using Akka.DependencyInjection;
using HTM.Communication.Actors;
using HTM.Core.Devices.Arduino.Actors;
using HTM.Infrastructure.Akka;

namespace HTM.Core.Actors;

public class HtmActor : BaseActor
{
    public HtmActor()
    {
        AddActor<TemperatureMeasurementActor>();
        AddActor<ArduinoActor>();
        AddActor<ArduinoMessengerActor>();
        AddActor<DevicesHealthMonitorActor>();
        AddActor<TemperatureMonitorActor>();
        AddActor<CommunicationActor>();
        AddActor<RequestHandlerActor>();
    }

    private void AddActor<T>() where T : BaseActor
    {
        Context.ActorOf(DependencyResolver.For(Context.System).Props<T>(), typeof(T).Name);
    }
}