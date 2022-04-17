using HTM.Infrastructure.MessagesBase;

namespace HTM.Infrastructure.Devices.Messages.Requests;

public class GetMessageByCommandHtmRequest : HtmRequest
{
    public SerialPortCommand Command { get; }
    
    public GetMessageByCommandHtmRequest(SerialPortCommand command)
    {
        Command = command;
    }
}