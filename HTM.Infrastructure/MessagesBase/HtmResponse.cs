namespace HTM.Infrastructure.MessagesBase;

public abstract class HtmResponse
{
    public Guid RequestId { get; }

    public HtmResponse(Guid requestId)
    {
        RequestId = requestId;
    }
}