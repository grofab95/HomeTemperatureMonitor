using HTM.Infrastructure.MessagesBase;

namespace HTM.Infrastructure.Devices.Messages.Requests;

public class SendMessageHtmRequest : HtmRequest
{
    public string Message { get; }

    public SendMessageHtmRequest(string message)
    {
        Message = message;
    }
}