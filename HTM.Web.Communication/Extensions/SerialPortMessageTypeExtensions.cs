using HTM.Communication.V2;
using HTM.Infrastructure.Models;

namespace HTM.Web.Communication.Extensions;

public static class SerialPortMessageTypeExtensions
{
    public static SerialPortMessageType ToSerialPortMessageType(this GrpcSerialPortMessageType messageType)
    {
        return messageType switch
        {
            GrpcSerialPortMessageType.Undefined => SerialPortMessageType.Undefined,
            GrpcSerialPortMessageType.Event => SerialPortMessageType.Event,
            GrpcSerialPortMessageType.CommandResponse => SerialPortMessageType.CommandResponse,

            _ => throw new ArgumentOutOfRangeException(nameof(messageType), messageType, null)
        };
    }

    public static GrpcSerialPortMessageType ToSerialPortMessageType(this SerialPortMessageType messageType)
    {
        return messageType switch
        {
            SerialPortMessageType.Undefined => GrpcSerialPortMessageType.Undefined,
            SerialPortMessageType.Event => GrpcSerialPortMessageType.Event,
            SerialPortMessageType.CommandResponse => GrpcSerialPortMessageType.CommandResponse,

            _ => throw new ArgumentOutOfRangeException(nameof(messageType), messageType, null)
        };
    }
}