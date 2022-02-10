namespace HTM.Infrastructure.Adapters;

public interface IDevice
{
    Task Initialize();
    event EventHandler<bool> ConnectionChanged;
}