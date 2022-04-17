using HTM.Infrastructure.Devices.Enums;
using HTM.Infrastructure.MessagesBase;

namespace HTM.Infrastructure.Devices.Messages.Requests;

public class GetDeviceConnectionStateRequest : HtmRequest
{
    public DeviceType DeviceType { get; }
    
    public GetDeviceConnectionStateRequest(DeviceType deviceType)
    {
        DeviceType = deviceType;
    }
}