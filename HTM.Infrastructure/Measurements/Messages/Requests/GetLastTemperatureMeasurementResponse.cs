using HTM.Infrastructure.MessagesBase;
using HTM.Infrastructure.Models;

namespace HTM.Infrastructure.Measurements.Messages.Requests;

public class GetLastTemperatureMeasurementResponse : HtmResponse
{
    public static GetLastTemperatureMeasurementResponse WithSuccess(Guid requestId, TemperatureMeasurement measurement) => new(requestId, measurement);
    public static GetLastTemperatureMeasurementResponse WithFailure(Guid requestId, Exception exception) => new(requestId, exception);
    
    public TemperatureMeasurement TemperatureMeasurement { get; }
    public Exception Exception { get; }
    
    public bool IsError => Exception != null;

    public GetLastTemperatureMeasurementResponse(Guid requestId, TemperatureMeasurement measurement) : base(requestId)
    {
        TemperatureMeasurement = measurement;
    }
    
    public GetLastTemperatureMeasurementResponse(Guid requestId, Exception exception) : base(requestId)
    {
        Exception = exception;
    }
}