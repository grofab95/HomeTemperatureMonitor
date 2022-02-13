using HTM.Infrastructure.MessagesBase;

namespace HTM.Core.Devices.Arduino.Messages.Requests;

public class SendMessageRequest : RequestBase
{
    public string? Message { get; }

    public SendMessageRequest(string? message)
    {
        Message = message;
    }
}