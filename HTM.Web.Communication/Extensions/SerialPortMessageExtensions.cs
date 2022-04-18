using HTM.Communication.V2;
using HTM.Infrastructure.Models;

namespace HTM.Web.Communication.Extensions;

public static class SerialPortMessageExtensions
{
    public static SerialPortMessage ToSerialPortMessage(this GrpcSerialPortMessage message)
    {
        return new SerialPortMessage(message.Type.ToSerialPortMessageType(), message.Text);
    }
    
    public static GrpcSerialPortMessage ToSerialPortMessage(this SerialPortMessage message)
    {
        return new GrpcSerialPortMessage
        {
            Type = message.Type.ToSerialPortMessageType(),
            Text = message.Text
        };
    }
}