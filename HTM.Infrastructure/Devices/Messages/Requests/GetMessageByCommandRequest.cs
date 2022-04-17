using HTM.Infrastructure.MessagesBase;

namespace HTM.Infrastructure.Devices.Messages.Requests;

public class GetMessageByCommandRequest : HtmRequest
{
    public SerialPortCommand Command { get; }
    
    public GetMessageByCommandRequest(SerialPortCommand command)
    {
        Command = command;
    }
}