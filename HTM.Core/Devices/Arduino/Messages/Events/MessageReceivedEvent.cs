using HTM.Infrastructure.MessagesBase;

namespace HTM.Core.Devices.Arduino.Messages.Events;

public class MessageReceivedEvent : EventBase
{
    public string Message { get; }

    public MessageReceivedEvent(string message)
    {
        Message = message;
    }
}