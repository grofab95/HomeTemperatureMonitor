using HTM.Infrastructure.MessagesBase;

namespace HTM.Infrastructure.Devices.Messages.Requests;

public class GetDeviceConnectionStateResponse : HtmResponse
{
    public bool IsConnected { get; }

    public GetDeviceConnectionStateResponse(Guid requestId, bool isConnected) : base(requestId)
    {
        IsConnected = isConnected;
    }
}