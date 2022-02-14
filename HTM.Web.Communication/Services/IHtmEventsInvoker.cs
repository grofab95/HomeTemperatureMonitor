using HTM.Infrastructure.Devices.Enums;

namespace HTM.Web.Communication.Services;

public interface IHtmEventsInvoker
{
    void InvokeDeviceConnectionChangedEvent(DeviceType deviceType, bool isConnected);
}