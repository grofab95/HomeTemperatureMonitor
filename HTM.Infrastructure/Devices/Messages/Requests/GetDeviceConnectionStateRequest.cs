using HTM.Infrastructure.Devices.Enums;

namespace HTM.Infrastructure.Devices.Messages.Requests;

public class GetDeviceConnectionStateRequest
{
    public DeviceType DeviceType { get; }
    
    public GetDeviceConnectionStateRequest(DeviceType deviceType)
    {
        DeviceType = deviceType;
    }
}