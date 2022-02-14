using HTM.Communication.V1;

namespace HTM.Communication.Extensions;

public static class DeviceTypeExtensions
{
    public static Infrastructure.Devices.Enums.DeviceType ToDeviceType(this DeviceType deviceType)
    {
        return deviceType switch
        {
            DeviceType.Arduino => Infrastructure.Devices.Enums.DeviceType.Arduino,
            
            _ => throw new ArgumentOutOfRangeException(nameof(deviceType), deviceType, null)
        };
    }
    
    public static DeviceType ToDeviceType(this Infrastructure.Devices.Enums.DeviceType deviceType)
    {
        return deviceType switch
        {
            Infrastructure.Devices.Enums.DeviceType.Arduino => DeviceType.Arduino,
            
            _ => throw new ArgumentOutOfRangeException(nameof(deviceType), deviceType, null)
        };
    }
}