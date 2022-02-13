using HTM.Core.Devices.Arduino.Messages.Requests;
using HTM.Infrastructure;
using HTM.Infrastructure.Messages.Events;

namespace HTM.Core.Actors;

public class TemperatureMonitorActor : BaseActor
{
    private readonly TimeSpan _loopDelay = TimeSpan.FromMilliseconds(300);
    public TemperatureMonitorActor()
    {
        Receive<TimerElapsedEvent>(_ =>
        {
            Context.System.EventStream.Publish(new GetMessageByCommandRequest(SerialPortCommand.GetTemperature));
        });
        
        Context.System.EventStream.Publish(new GetMessageByCommandRequest(SerialPortCommand.GetTemperature));

        Receive<GetMessageByCommandResponse>(OnGetMessageByCommandResponse);
        
        Context.System.Scheduler.ScheduleTellRepeatedly(TimeSpan.Zero, _loopDelay, Self, TimerElapsedEvent.Instance, Self);
    }

    private void OnGetMessageByCommandResponse(GetMessageByCommandResponse response)
    {
        
    }
}