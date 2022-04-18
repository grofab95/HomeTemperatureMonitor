using HTM.Infrastructure.MessagesBase;
using HTM.Infrastructure.Models;

namespace HTM.Infrastructure.Devices.Messages.Events;

public class SerialPortMessageReceivedEvent : EventBase
{
    public SerialPortMessage[] Messages { get; }

    public SerialPortMessageReceivedEvent(SerialPortMessage[] messages)
    {
        Messages = messages;
    }
}