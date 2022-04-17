using HTM.Infrastructure.MessagesBase;
using HTM.Infrastructure.Models;

namespace HTM.Infrastructure.Measurements.Messages.Requests;

public class GetTemperatureMeasurementsByDateRangeResponse : HtmResponse
{
    public static GetTemperatureMeasurementsByDateRangeResponse WithSuccess(Guid requestId, TemperatureMeasurement[] measurements) => new(requestId, measurements);
    public static GetTemperatureMeasurementsByDateRangeResponse WithFailure(Guid requestId, Exception exception) => new(requestId, exception);

    public TemperatureMeasurement[] TemperatureMeasurements { get; }
    public Exception Exception { get; } = null!;
    public bool IsError => Exception != null;

    public GetTemperatureMeasurementsByDateRangeResponse(Guid requestId, TemperatureMeasurement[] measurements) : base(requestId)
    {
        TemperatureMeasurements = measurements;
    }
    
    public GetTemperatureMeasurementsByDateRangeResponse(Guid requestId, Exception exception) : base(requestId)
    {
        Exception = exception;
    }
}