namespace HTM.Infrastructure.Devices.Messages.Requests;

public class GetDeviceConnectionStateResponse
{
    public bool IsConnected { get; }

    public GetDeviceConnectionStateResponse(bool isConnected)
    {
        IsConnected = isConnected;
    }
}