using HTM.Infrastructure.Devices.Enums;
using HTM.Infrastructure.Models;

namespace HTM.Web.Communication.Services;

public interface IHtmEventsInvoker
{
    void InvokeDeviceConnectionChangedEvent(DeviceType deviceType, bool isConnected);
    void InvokeSerialPortMessagesReceivedEvent(SerialPortMessage[] messages);
}