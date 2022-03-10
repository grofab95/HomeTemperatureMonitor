using Google.Protobuf.WellKnownTypes;
using HTM.Communication.V1;
using HTM.Infrastructure.Models;

namespace HTM.Communication.Extensions;

public static class TemperatureMeasurementExtensions
{
    public static GrpcTemperatureMeasurement ToGrpcTemperatureMeasurement(this TemperatureMeasurement measurement)
    {
        return new GrpcTemperatureMeasurement
        {
            Id = measurement.Id,
            Temperature = measurement.Temperature,
            MeasurementDate = Timestamp.FromDateTime(DateTime.SpecifyKind(measurement.MeasurementDate, DateTimeKind.Utc))
        };
    }
}