namespace HTM.Infrastructure.Messages.Events;

public class TemperatureMeasurementActorInitializedEvent
{
    public static TemperatureMeasurementActorInitializedEvent Instance => new();

    private TemperatureMeasurementActorInitializedEvent()
    { }
}