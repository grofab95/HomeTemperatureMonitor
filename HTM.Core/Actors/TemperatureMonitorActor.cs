using HTM.Communication.V1;
using HTM.Infrastructure;
using HTM.Infrastructure.Akka;
using HTM.Infrastructure.Messages.Events;
using GetMessageByCommandRequest = HTM.Infrastructure.Devices.Messages.Requests.GetMessageByCommandRequest;
using GetMessageByCommandResponse = HTM.Infrastructure.Devices.Messages.Requests.GetMessageByCommandResponse;

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