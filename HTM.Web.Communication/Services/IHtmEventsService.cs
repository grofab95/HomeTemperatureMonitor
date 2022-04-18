using HTM.Infrastructure.Devices.Enums;
using HTM.Infrastructure.Models;

namespace HTM.Web.Communication.Services;

public interface IHtmEventsService
{
    event EventHandler<(DeviceType deviceType, bool isConnected)> OnDeviceConnectionChangedEvent;
    event EventHandler<SerialPortMessage[]> OnSerialPortMessagesReceivedEvent;
}