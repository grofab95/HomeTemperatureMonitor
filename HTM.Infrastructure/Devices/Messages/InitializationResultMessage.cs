namespace HTM.Infrastructure.Devices.Messages;

public class InitializationResultMessage
{
    public static InitializationResultMessage WithSuccess => new();
    public static InitializationResultMessage WithError(Exception exception) => new(exception);
    
    public bool IsError => Exception != null;
    public Exception Exception { get; }

    private InitializationResultMessage(Exception exception)
    {
        Exception = exception;
    }

    private InitializationResultMessage()
    {
        
    }
}