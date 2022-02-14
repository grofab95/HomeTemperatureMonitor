using HTM.Infrastructure.Devices.Enums;

namespace HTM.Web.Communication.Services;

public interface IHtmEventsService
{
    event EventHandler<(DeviceType deviceType, bool isConnected)> OnDeviceConnectionChangedEvent;
}