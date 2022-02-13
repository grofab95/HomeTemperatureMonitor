using HTM.Infrastructure;
using HTM.Infrastructure.MessagesBase;

namespace HTM.Core.Devices.Arduino.Messages.Requests;

public class GetMessageByCommandRequest : RequestBase
{
    public SerialPortCommand Command { get; }
    
    public GetMessageByCommandRequest(SerialPortCommand command)
    {
        Command = command;
    }
}