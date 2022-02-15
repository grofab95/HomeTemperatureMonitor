using HTM.Infrastructure.Models;

namespace HTM.Database.Entities;

public class TemperatureMeasurementDb : EntityBase
{
    public DateTime MeasurementDate { get; set; }
    public float Temperature { get; set; }

    private TemperatureMeasurementDb()
    { }
    
    public TemperatureMeasurementDb(TemperatureMeasurement temperatureMeasurement)
    {
        MeasurementDate = temperatureMeasurement.MeasurementDate;
        Temperature = temperatureMeasurement.Temperature;
    }
}