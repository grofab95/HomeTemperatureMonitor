using HTM.Infrastructure.Devices.Enums;
using HTM.Infrastructure.Models;

namespace HTM.Web.Communication.Services;

public class HtmEventsService : IHtmEventsService, IHtmEventsInvoker
{
    public event EventHandler<(DeviceType deviceType, bool isConnected)> OnDeviceConnectionChangedEvent;
    public event EventHandler<SerialPortMessage[]> OnSerialPortMessagesReceivedEvent;

    public void InvokeDeviceConnectionChangedEvent(DeviceType deviceType, bool isConnected)
    {
        OnDeviceConnectionChangedEvent?.Invoke(this, new (deviceType, isConnected));
    }

    public void InvokeSerialPortMessagesReceivedEvent(SerialPortMessage[] messages)
    {
        OnSerialPortMessagesReceivedEvent?.Invoke(this, messages);
    }
}