using HTM.Infrastructure.MessagesBase;

namespace HTM.Infrastructure.Devices.Messages.Requests;

public class SendMessageResponse : ResponseBase
{
    public Exception Exception { get; }
    public bool IsError => Exception != null;

    public SendMessageResponse(string requestId) : base(requestId)
    { }
    
    public SendMessageResponse(string requestId, Exception exception) : base(requestId)
    {
        Exception = exception;
    }
}