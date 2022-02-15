using HTM.Infrastructure.Models;

namespace HTM.Infrastructure.Measurements.Messages.Requests;

public class GetTemperatureMeasurementsByDateRangeResponse
{
    public static GetTemperatureMeasurementsByDateRangeResponse WithSuccess(TemperatureMeasurement?[] measurements) => new(measurements);
    public static GetTemperatureMeasurementsByDateRangeResponse WithFailure(Exception exception) => new(exception);

    public TemperatureMeasurement[]? TemperatureMeasurements { get; }
    public Exception Exception { get; } = null!;
    public bool IsError => Exception != null;

    public GetTemperatureMeasurementsByDateRangeResponse(TemperatureMeasurement?[] measurements)
    {
        TemperatureMeasurements = measurements;
    }
    
    public GetTemperatureMeasurementsByDateRangeResponse(Exception exception)
    {
        Exception = exception;
    }
}