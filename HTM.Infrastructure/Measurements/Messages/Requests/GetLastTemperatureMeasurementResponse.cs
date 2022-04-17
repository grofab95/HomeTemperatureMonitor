using HTM.Infrastructure.Models;

namespace HTM.Infrastructure.Measurements.Messages.Requests;

public class GetLastTemperatureMeasurementResponse
{
    public static GetLastTemperatureMeasurementResponse WithSuccess(TemperatureMeasurement measurement) => new(measurement);
    public static GetLastTemperatureMeasurementResponse WithFailure(Exception exception) => new(exception);
    
    public TemperatureMeasurement TemperatureMeasurement { get; }
    public Exception Exception { get; }
    
    public bool IsError => Exception != null;

    public GetLastTemperatureMeasurementResponse(TemperatureMeasurement measurement)
    {
        TemperatureMeasurement = measurement;
    }
    
    public GetLastTemperatureMeasurementResponse(Exception exception)
    {
        Exception = exception;
    }
}