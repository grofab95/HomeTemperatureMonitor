using HTM.Infrastructure.MessagesBase;

namespace HTM.Infrastructure.Measurements.Messages.Requests;

public class AddTemperatureMeasurementResponse : HtmResponse
{
    public static AddTemperatureMeasurementResponse WithSuccess(Guid requestId) => new(requestId);
    public static AddTemperatureMeasurementResponse WithFailure(Guid requestId, Exception exception) => new(requestId, exception);
    
    public Exception Exception { get; }
    public bool IsError => Exception != null;

    public AddTemperatureMeasurementResponse(Guid requestId, Exception exception = null) : base(requestId)
    {
        Exception = exception;
    }
}