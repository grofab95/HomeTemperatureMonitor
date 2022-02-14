using HTM.Communication.V1;

namespace HTM.Web.Communication.Extensions;

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
}