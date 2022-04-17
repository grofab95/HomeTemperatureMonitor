using HTM.Infrastructure.MessagesBase;

namespace HTM.Infrastructure.Measurements.Messages.Requests;

public class GetTemperatureMeasurementsByDateRangeHtmRequest : HtmRequest
{
    public DateTime From { get; }
    public DateTime To { get; }

    public GetTemperatureMeasurementsByDateRangeHtmRequest(DateTime from, DateTime to)
    {
        From = from;
        To = to;
    }
}