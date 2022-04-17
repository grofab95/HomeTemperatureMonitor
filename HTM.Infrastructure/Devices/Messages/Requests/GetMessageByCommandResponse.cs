using HTM.Infrastructure.MessagesBase;

namespace HTM.Infrastructure.Devices.Messages.Requests;

public class GetMessageByCommandResponse : HtmResponse
{
    public string Message { get; }
    public Exception Exception { get; }
    public bool IsError => Exception != null;

    public GetMessageByCommandResponse(Guid requestId, string message) : base(requestId)
    {
        Message = message;
    }
    
    public GetMessageByCommandResponse(Guid requestId, Exception exception) : base(requestId)
    {
        Exception = exception;
    }
}