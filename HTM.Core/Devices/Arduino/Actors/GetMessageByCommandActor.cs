﻿using Akka.Actor;
using Akka.Event;
using HTM.Infrastructure.Akka;
using HTM.Infrastructure.Devices.Messages.Events;
using HTM.Infrastructure.Devices.Messages.Requests;
using HTM.Infrastructure.Messages.Events;

namespace HTM.Core.Devices.Arduino.Actors;

public class GetMessageByCommandActor : BaseActor
{
    private readonly GetMessageByCommandRequest _request;
    private const int TimeoutInSec = 5;

    private readonly ICancelable _cancelable;

    public GetMessageByCommandActor(GetMessageByCommandRequest request)
    {
        _request = request;
        Context.System.EventStream.Publish(new SendMessageRequest(request.Command.ToString()));
        _cancelable = Context.System.Scheduler.ScheduleTellOnceCancelable(TimeSpan.FromSeconds(TimeoutInSec), Self, TimeoutEvent.Instance, Self);

        Receive<TimeoutEvent>(_ => SendMessageToParent(new TimeoutException($"Timeout after {TimeoutInSec} s.")));

        Receive<SendMessageResponse>(response =>
        {
            if (response.IsError)
            {
                SendMessageToParent(response.Exception);
            }
        });

        Receive<MessageReceivedEvent>(e => SendMessageToParent(e.Message));

        Context.System.EventStream.Subscribe<MessageReceivedEvent>(Self);
    }

    private void SendMessageToParent(string message) => SendMessageToParent(new GetMessageByCommandResponse(_request.RequestId, message));
    private void SendMessageToParent(Exception exception) => SendMessageToParent(new GetMessageByCommandResponse(_request.RequestId, exception));
    
    private void SendMessageToParent(GetMessageByCommandResponse response)
    {
        Context.System.EventStream.Unsubscribe<MessageReceivedEvent>(Self);
        _cancelable.Cancel();
        Context.Parent.Tell(response);
        
        StopSelf();
    }
}