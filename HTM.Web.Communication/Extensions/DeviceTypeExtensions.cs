using HTM.Communication.V1;
using HTM.Infrastructure.Devices.Enums;

namespace HTM.Web.Communication.Extensions;

public static class DeviceTypeExtensions
{
    public static DeviceType ToDeviceType(this GrpcDeviceType deviceType)
    {
        return deviceType switch
        {
            GrpcDeviceType.Arduino => DeviceType.Arduino,
            
            _ => throw new ArgumentOutOfRangeException(nameof(deviceType), deviceType, null)
        };
    }
    
    public static GrpcDeviceType ToDeviceType(this DeviceType deviceType)
    {
        return deviceType switch
        {
            DeviceType.Arduino => GrpcDeviceType.Arduino,
            
            _ => throw new ArgumentOutOfRangeException(nameof(deviceType), deviceType, null)
        };
    }
}