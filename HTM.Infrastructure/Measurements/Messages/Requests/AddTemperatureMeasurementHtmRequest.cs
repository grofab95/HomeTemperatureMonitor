using HTM.Infrastructure.MessagesBase;
using HTM.Infrastructure.Models;

namespace HTM.Infrastructure.Measurements.Messages.Requests;

public class AddTemperatureMeasurementHtmRequest : HtmRequest
{
    public TemperatureMeasurement TemperatureMeasurement { get; }

    public AddTemperatureMeasurementHtmRequest(TemperatureMeasurement temperatureMeasurement)
    {
        TemperatureMeasurement = temperatureMeasurement;
    }
}