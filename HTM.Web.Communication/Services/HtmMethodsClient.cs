using Grpc.Net.Client;
using HTM.Communication.V1;
using HTM.Infrastructure;
using HTM.Web.Communication.Extensions;
using Serilog;
using DeviceType = HTM.Infrastructure.Devices.Enums.DeviceType;

namespace HTM.Web.Communication.Services;

public class HtmMethodsClient
{
    private readonly GrpcChannel _grpcChannel;
    
    public HtmMethodsClient()
    {
        _grpcChannel = GrpcChannel.ForAddress("http://localhost:2010");
    }

    public async Task<bool> GetDeviceConnectionStatus(DeviceType deviceType)
    {
        try
        {
            var client = new HTMMethodsService.HTMMethodsServiceClient(_grpcChannel);
            var response = await client.GetDeviceConnectionStateAsync(new GrpcGetDeviceConnectionStateRequest
            {
                DeviceType = deviceType.ToDeviceType()
            });

            return response.IsConnected;
        }
        catch (Exception ex)
        {
            Log.Error(ex, nameof(GetDeviceConnectionStatus));
            return false;
        }
    }
    
    public async Task<string> GetMessageByCommand(SerialPortCommand command)
    {
        try
        {
            var client = new HTMMethodsService.HTMMethodsServiceClient(_grpcChannel);
            var response = await client.GetMessageByCommandAsync(new GrpcGetMessageByCommandRequest
            {
                Command = command.ToString()
            });

            return response.Message;
        }
        catch (Exception ex)
        {
            Log.Error(ex, nameof(GetDeviceConnectionStatus));
            return ex.Message;
        }
    }
}