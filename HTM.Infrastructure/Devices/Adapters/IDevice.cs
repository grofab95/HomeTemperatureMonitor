namespace HTM.Infrastructure.Devices.Adapters;

public interface IDevice
{
    Task Initialize();
    event EventHandler<bool> ConnectionChanged;
}