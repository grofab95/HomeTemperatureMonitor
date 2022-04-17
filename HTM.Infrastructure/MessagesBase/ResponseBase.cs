namespace HTM.Infrastructure.MessagesBase;

public abstract class ResponseBase
{
    public string RequestId { get; }

    public ResponseBase(string requestId)
    {
        RequestId = requestId;
    }
}