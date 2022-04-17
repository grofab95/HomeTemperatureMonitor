using HTM.Infrastructure.MessagesBase;

namespace HTM.Infrastructure.Devices.Messages.Requests;

public class SendMessageResponse : HtmResponse
{
    public Exception Exception { get; }
    public bool IsError => Exception != null;

    public SendMessageResponse(Guid requestId) : base(requestId)
    { }
    
    public SendMessageResponse(Guid requestId, Exception exception) : base(requestId)
    {
        Exception = exception;
    }
}