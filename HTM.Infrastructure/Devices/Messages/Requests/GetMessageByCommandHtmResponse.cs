using HTM.Infrastructure.MessagesBase;

namespace HTM.Infrastructure.Devices.Messages.Requests;

public class GetMessageByCommandHtmResponse : HtmResponse
{
    public string Message { get; }
    public Exception Exception { get; }
    public bool IsError => Exception != null;

    public GetMessageByCommandHtmResponse(Guid requestId, string message) : base(requestId)
    {
        Message = message;
    }
    
    public GetMessageByCommandHtmResponse(Guid requestId, Exception exception) : base(requestId)
    {
        Exception = exception;
    }
}