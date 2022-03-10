namespace HTM.Infrastructure.Measurements.Messages.Requests;

public class GetTemperatureMeasurementsByDateRangeRequest
{
    public DateTime From { get; }
    public DateTime To { get; }

    public GetTemperatureMeasurementsByDateRangeRequest(DateTime from, DateTime to)
    {
        From = from;
        To = to;
    }
}