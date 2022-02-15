namespace HTM.Infrastructure.Measurements.Messages.Requests;

public class AddTemperatureMeasurementResponse
{
    public static AddTemperatureMeasurementResponse WithSuccess() => new();
    public static AddTemperatureMeasurementResponse WithFailure(Exception exception) => new(exception);
    
    public Exception Exception { get; }
    public bool IsError => Exception != null;

    public AddTemperatureMeasurementResponse(Exception exception = null)
    {
        Exception = exception;
    }
}