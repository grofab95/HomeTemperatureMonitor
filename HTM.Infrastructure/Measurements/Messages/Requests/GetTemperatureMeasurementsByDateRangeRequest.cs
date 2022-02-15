namespace HTM.Infrastructure.Measurements.Messages.Requests;

public class GetTemperatureMeasurementsByDateRangeRequest
{
    public DateTime From { get; set; }
    public DateTime To { get; set; }
}