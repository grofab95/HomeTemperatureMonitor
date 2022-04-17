using HTM.Infrastructure.MessagesBase;

namespace HTM.Infrastructure.Devices.Messages.Requests;

public class SendMessageRequest : HtmRequest
{
    public string Message { get; }

    public SendMessageRequest(string message)
    {
        Message = message;
    }
}