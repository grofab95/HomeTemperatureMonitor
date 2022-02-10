namespace HTM.Infrastructure.MessagesBase;

public abstract class RequestBase
{
    public string RequestId { get; } = Guid.NewGuid().ToString();
}