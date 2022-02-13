namespace HTM.Infrastructure.Messages.Events;

public class TimeoutEvent
{
    public static TimeoutEvent Instance => new();
    
    private TimeoutEvent()
    { }
}