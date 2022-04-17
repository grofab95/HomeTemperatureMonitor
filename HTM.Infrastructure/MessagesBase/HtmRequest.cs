namespace HTM.Infrastructure.MessagesBase;

public abstract class HtmRequest
{
    public Guid RequestId { get; } = Guid.NewGuid();
}