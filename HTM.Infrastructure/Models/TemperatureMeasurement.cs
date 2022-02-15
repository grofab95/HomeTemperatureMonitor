namespace HTM.Infrastructure.Models;

public class TemperatureMeasurement
{
    public long Id { get; set; }
    public DateTime MeasurementDate { get; set; }
    public float Temperature { get; set; }
}