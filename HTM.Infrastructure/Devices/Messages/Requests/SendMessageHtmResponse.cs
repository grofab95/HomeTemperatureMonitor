using HTM.Infrastructure.MessagesBase;

namespace HTM.Infrastructure.Devices.Messages.Requests;

public class SendMessageHtmResponse : HtmResponse
{
    public Exception Exception { get; }
    public bool IsError => Exception != null;

    public SendMessageHtmResponse(Guid requestId) : base(requestId)
    { }
    
    public SendMessageHtmResponse(Guid requestId, Exception exception) : base(requestId)
    {
        Exception = exception;
    }
}