using HTM.Infrastructure.MessagesBase;

namespace HTM.Infrastructure.Devices.Messages.Requests;

public class GetMessageByCommandResponse : ResponseBase
{
    public string? Message { get; }
    public Exception? Exception { get; }
    public bool IsError => Exception != null;

    public GetMessageByCommandResponse(string? requestId, string message) : base(requestId)
    {
        Message = message;
    }
    
    public GetMessageByCommandResponse(string? requestId, Exception? exception) : base(requestId)
    {
        Exception = exception;
    }
}