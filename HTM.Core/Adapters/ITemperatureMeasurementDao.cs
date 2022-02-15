using HTM.Infrastructure.Models;

namespace HTM.Core.Adapters;

public interface ITemperatureMeasurementDao
{
    Task AddMeasurement(TemperatureMeasurement temperatureMeasurement);
    Task<TemperatureMeasurement?> GetLastMeasurement();
    Task<TemperatureMeasurement?[]> GetMeasurementsByDateRange(DateTime from, DateTime to);
}