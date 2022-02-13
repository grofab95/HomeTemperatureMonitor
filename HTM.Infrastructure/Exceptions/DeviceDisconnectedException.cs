namespace HTM.Infrastructure.Exceptions;

public class DeviceDisconnectedException : Exception
{
    private new const string Message = "Device is disconnected.";

    public DeviceDisconnectedException() : base(Message)
    { }
}