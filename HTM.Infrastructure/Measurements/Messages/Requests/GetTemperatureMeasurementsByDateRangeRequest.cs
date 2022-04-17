using HTM.Infrastructure.MessagesBase;

namespace HTM.Infrastructure.Measurements.Messages.Requests;

public class GetTemperatureMeasurementsByDateRangeRequest : HtmRequest
{
    public DateTime From { get; }
    public DateTime To { get; }

    public GetTemperatureMeasurementsByDateRangeRequest(DateTime from, DateTime to)
    {
        From = from;
        To = to;
    }
}