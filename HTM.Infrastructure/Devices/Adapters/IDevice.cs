namespace HTM.Infrastructure.Devices.Adapters;

public interface IDevice : IDisposable
{
    Task Initialize();
    event EventHandler<bool> ConnectionChanged;
}