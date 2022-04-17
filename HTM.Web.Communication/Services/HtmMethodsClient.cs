using Google.Protobuf.WellKnownTypes;
using Grpc.Net.Client;
using HTM.Communication.V1;
using HTM.Infrastructure;
using HTM.Infrastructure.Devices.Enums;
using HTM.Infrastructure.Models;
using HTM.Web.Communication.Extensions;
using Serilog;

namespace HTM.Web.Communication.Services;

public class HtmMethodsClient
{
    private readonly GrpcChannel _grpcChannel;
    
    private DateTime DeadLine => DateTime.UtcNow.AddSeconds(5);
    
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
            }, deadline: DeadLine);

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
        var client = new HTMMethodsService.HTMMethodsServiceClient(_grpcChannel);
        var response = await client.GetMessageByCommandAsync(new GrpcGetMessageByCommandRequest
        {
            Command = command.ToString()
        }, deadline: DeadLine);

        return response.Message;
    }
    
    public async Task<TemperatureMeasurement[]> GetTemperaturesMeasurements(DateTime from, DateTime to)
    {
        try
        {
            var client = new HTMMethodsService.HTMMethodsServiceClient(_grpcChannel);
            var response = await client.GrpcGetTemperatureMeasurementsAsync(new GrpcGetTemperatureMeasurementsRequest
            {
                From = Timestamp.FromDateTime(DateTime.SpecifyKind(from, DateTimeKind.Utc)),
                To = Timestamp.FromDateTime(DateTime.SpecifyKind(to, DateTimeKind.Utc))
            }, deadline: DeadLine);

            return response.GrpcTemperatureMeasurements.Select(x => x.ToTemperaturesMeasurement()).ToArray();
        }
        catch (Exception ex)
        {
            Log.Error(ex, nameof(GetDeviceConnectionStatus));
            return null;
        }
    }
}