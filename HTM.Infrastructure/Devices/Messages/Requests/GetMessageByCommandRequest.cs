using HTM.Infrastructure.MessagesBase;
using HTM.Infrastructure.Models;

namespace HTM.Infrastructure.Devices.Messages.Requests;

public class GetMessageByCommandRequest : HtmRequest
{
    public SerialPortCommand Command { get; }
    
    public GetMessageByCommandRequest(SerialPortCommand command)
    {
        Command = command;
    }
}