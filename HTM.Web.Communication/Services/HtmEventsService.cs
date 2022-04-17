using HTM.Infrastructure.Devices.Enums;

namespace HTM.Web.Communication.Services;

public class HtmEventsService : IHtmEventsService, IHtmEventsInvoker
{
    public event EventHandler<(DeviceType deviceType, bool isConnected)> OnDeviceConnectionChangedEvent;
    
    public void InvokeDeviceConnectionChangedEvent(DeviceType deviceType, bool isConnected)
    {
        OnDeviceConnectionChangedEvent?.Invoke(this, new (deviceType, isConnected));
    }
}