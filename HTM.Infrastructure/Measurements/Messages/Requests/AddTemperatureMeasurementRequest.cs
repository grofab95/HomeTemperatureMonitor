using HTM.Infrastructure.Models;

namespace HTM.Infrastructure.Measurements.Messages.Requests;

public class AddTemperatureMeasurementRequest
{
    public TemperatureMeasurement TemperatureMeasurement { get; }

    public AddTemperatureMeasurementRequest(TemperatureMeasurement temperatureMeasurement)
    {
        TemperatureMeasurement = temperatureMeasurement;
    }
}