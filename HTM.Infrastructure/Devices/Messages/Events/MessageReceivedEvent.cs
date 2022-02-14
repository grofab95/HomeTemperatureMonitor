using HTM.Infrastructure.MessagesBase;

namespace HTM.Infrastructure.Devices.Messages.Events;

public class MessageReceivedEvent : EventBase
{
    public string Message { get; }

    public MessageReceivedEvent(string message)
    {
        Message = message;
    }
}