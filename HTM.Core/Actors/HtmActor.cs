using Akka.DependencyInjection;
using HTM.Core.Devices.Arduino.Actors;
using HTM.Infrastructure.Devices.Messages.Events;

namespace HTM.Core.Actors;

public class HtmActor : BaseActor
{
    public HtmActor()
    {
       
    }

    protected override void PreStart()
    {
        AddTopLevelActor<TemperatureMonitorActor>();
        AddTopLevelActor<ArduinoActor>();
    }
    
    private void AddTopLevelActor<T>() where T : BaseActor
    {
        Context.ActorOf(DependencyResolver.For(Context.System).Props<T>(), typeof(T).Name);
    }
}