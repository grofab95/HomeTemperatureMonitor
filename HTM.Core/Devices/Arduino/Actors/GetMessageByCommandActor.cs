using Akka.Actor;
using Akka.Event;
using HTM.Core.Actors;
using HTM.Infrastructure.Akka;
using HTM.Infrastructure.Devices.Messages.Events;
using HTM.Infrastructure.Devices.Messages.Requests;
using HTM.Infrastructure.Messages.Events;

namespace HTM.Core.Devices.Arduino.Actors;

public class GetMessageByCommandActor : BaseActor
{
    private readonly GetMessageByCommandHtmRequest _htmRequest;
    private const int TimeoutInSec = 5;

    private readonly ICancelable _cancelable;

    public GetMessageByCommandActor(GetMessageByCommandHtmRequest htmRequest)
    {
        _htmRequest = htmRequest;
        Context.System.EventStream.Publish(new SendMessageHtmRequest(htmRequest.Command.ToString()));
        _cancelable = Context.System.Scheduler.ScheduleTellOnceCancelable(TimeSpan.FromSeconds(TimeoutInSec), Self, TimeoutEvent.Instance, Self);

        Receive<TimeoutEvent>(_ => SendMessageToParent(new TimeoutException($"Timeout after {TimeoutInSec} s.")));

        Receive<SendMessageHtmResponse>(response =>
        {
            if (response.IsError)
            {
                SendMessageToParent(response.Exception);
            }
        });

        Receive<MessageReceivedEvent>(e => SendMessageToParent(e.Message));

        Context.System.EventStream.Subscribe<MessageReceivedEvent>(Self);
    }

    private void SendMessageToParent(string message) => SendMessageToParent(new GetMessageByCommandHtmResponse(_htmRequest.RequestId, message));
    private void SendMessageToParent(Exception exception) => SendMessageToParent(new GetMessageByCommandHtmResponse(_htmRequest.RequestId, exception));
    
    private void SendMessageToParent(GetMessageByCommandHtmResponse htmResponse)
    {
        Context.System.EventStream.Unsubscribe<MessageReceivedEvent>(Self);
        _cancelable.Cancel();
        Context.Parent.Tell(htmResponse);
        
        StopSelf();
    }
}