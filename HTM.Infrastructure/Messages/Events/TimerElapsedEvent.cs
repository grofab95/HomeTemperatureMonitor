namespace HTM.Infrastructure.Messages.Events;

public class TimerElapsedEvent
{
    public static TimerElapsedEvent Instance => new();

    private TimerElapsedEvent()
    { }
}