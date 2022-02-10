using Akka.Actor;
using Akka.Event;
using HTM.Infrastructure.Devices.Messages.Requests;
using HTM.Infrastructure.Messages.Events;

namespace HTM.Core.Actors;

public class TemperatureMonitorActor : BaseActor
{
    private readonly TimeSpan _loopDelay = TimeSpan.FromMinutes(15);
    
    public TemperatureMonitorActor()
    {
        Receive<TimerElapsedEvent>(_ =>
        {
            
            Context.System.EventStream.Publish(new GetTemperatureRequest());
        });
        Receive<GetTemperatureResponse>(HandleGetTemperatureResponse);

        Context.System.EventStream.Subscribe<GetTemperatureResponse>(Self);
        
        Context.System.Scheduler.ScheduleTellRepeatedly(TimeSpan.FromSeconds(15), _loopDelay, Self, TimerElapsedEvent.Instance, Self);
    }

    private void HandleGetTemperatureResponse(GetTemperatureResponse response)
    {
     
    }
}