using HTM.Infrastructure.Devices.Enums;
using HTM.Infrastructure.MessagesBase;

namespace HTM.Infrastructure.Devices.Messages.Requests;

public class GetDeviceConnectionStateHtmRequest : HtmRequest
{
    public DeviceType DeviceType { get; }
    
    public GetDeviceConnectionStateHtmRequest(DeviceType deviceType)
    {
        DeviceType = deviceType;
    }
}