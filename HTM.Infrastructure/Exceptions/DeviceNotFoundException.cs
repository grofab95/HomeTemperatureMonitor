using HTM.Infrastructure.Devices.Enums;

namespace HTM.Infrastructure.Exceptions;

public sealed class DeviceNotFoundException : Exception
{
    public DeviceNotFoundException(DeviceType deviceType) : base($"{deviceType} not found.")
    { }
}