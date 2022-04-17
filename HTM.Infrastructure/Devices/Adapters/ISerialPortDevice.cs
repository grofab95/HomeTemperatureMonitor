namespace HTM.Infrastructure.Devices.Adapters;

public interface ISerialPortDevice : IDevice
{
    void SendMessage(string message);
    event EventHandler<string> OnMessageReceived;
}