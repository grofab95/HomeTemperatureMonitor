using HTM.Communication.V1;
using HTM.Infrastructure.Models;

namespace HTM.Web.Communication.Extensions;

public static class TemperaturesMeasurementExtensions
{
    public static TemperatureMeasurement ToTemperaturesMeasurement(this GrpcTemperatureMeasurement temperatureMeasurement)
    {
        return new TemperatureMeasurement
        {
            Id = temperatureMeasurement.Id,
            Temperature = temperatureMeasurement.Temperature,
            MeasurementDate = temperatureMeasurement.MeasurementDate.ToDateTime()
        };
    }
}