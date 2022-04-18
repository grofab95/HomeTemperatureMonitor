using Akka.Actor;
using Grpc.Core;
using HTM.Communication.Extensions;
using HTM.Communication.V2;
using HTM.Infrastructure.Akka;
using HTM.Infrastructure.Devices.Messages.Requests;
using HTM.Infrastructure.Measurements.Messages.Requests;
using HTM.Infrastructure.Models;
using Serilog;

namespace HTM.Communication.Services;

public class HtmMethodsServer : V2.HTMMethodsService.HTMMethodsServiceBase
{
    private readonly HtmActorBridge _htmActorBridge;

    public HtmMethodsServer(HtmActorBridge htmActorBridge)
    {
        _htmActorBridge = htmActorBridge;
    }
    
    public override async Task<GrpcGetDeviceConnectionStateResponse> GetDeviceConnectionState(GrpcGetDeviceConnectionStateRequest request, ServerCallContext context)
    {
        Log.Information("HtmMethodsServer | GetDeviceConnectionState, Device={Device}", request.DeviceType);
        
        var response = await _htmActorBridge.RequestHandlerActor
            .Ask<GetDeviceConnectionStateResponse>(
                new GetDeviceConnectionStateRequest(request.DeviceType.ToDeviceType()));

        return new GrpcGetDeviceConnectionStateResponse
        {
            IsConnected = response.IsConnected
        };
    }

    public override async Task<GrpcGetMessageByCommandResponse> GetMessageByCommand(GrpcGetMessageByCommandRequest request, ServerCallContext context)
    {
        Log.Information("HtmMethodsServer | GetMessageByCommand, Command={Command}", request.Command);
        
        var response = await _htmActorBridge.RequestHandlerActor
            .Ask<GetMessageByCommandResponse>(
                new GetMessageByCommandRequest(Enum.Parse<SerialPortCommand>(request.Command)));

        return new GrpcGetMessageByCommandResponse
        {
          Message  = response.Message
        };
    }

    public override async Task<GrpcGetTemperatureMeasurementsResponse> GrpcGetTemperatureMeasurements(GrpcGetTemperatureMeasurementsRequest request, ServerCallContext context)
    {
        Log.Information("HtmMethodsServer | GetTemperatureMeasurements");
        
        var from = request.From.ToDateTime();
        var to = request.To.ToDateTime();
        
        var response = await _htmActorBridge.RequestHandlerActor
            .Ask<GetTemperatureMeasurementsByDateRangeResponse>(
                new GetTemperatureMeasurementsByDateRangeRequest(from, to));

        var grpcResponse = new GrpcGetTemperatureMeasurementsResponse();
        grpcResponse.GrpcTemperatureMeasurements.AddRange(
            response.TemperatureMeasurements?.Select(x => x.ToGrpcTemperatureMeasurement()));

        return grpcResponse;
    }
}