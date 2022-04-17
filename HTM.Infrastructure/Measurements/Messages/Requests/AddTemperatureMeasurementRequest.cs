using HTM.Infrastructure.MessagesBase;
using HTM.Infrastructure.Models;

namespace HTM.Infrastructure.Measurements.Messages.Requests;

public class AddTemperatureMeasurementRequest : HtmRequest
{
    public TemperatureMeasurement TemperatureMeasurement { get; }

    public AddTemperatureMeasurementRequest(TemperatureMeasurement temperatureMeasurement)
    {
        TemperatureMeasurement = temperatureMeasurement;
    }
}