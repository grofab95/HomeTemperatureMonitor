using Akka.Actor;
using Grpc.Core;
using HTM.Communication.Extensions;
using HTM.Communication.V1;
using HTM.Infrastructure;
using HTM.Infrastructure.Akka;
using HTM.Infrastructure.Devices.Messages.Requests;
using HTM.Infrastructure.Measurements.Messages.Requests;

namespace HTM.Communication.Services;

public class HtmMethodsServer : V1.HTMMethodsService.HTMMethodsServiceBase
{
    private readonly HtmActorBridge _htmActorBridge;

    public HtmMethodsServer(HtmActorBridge htmActorBridge)
    {
        _htmActorBridge = htmActorBridge;
    }
    
    public override async Task<GrpcGetDeviceConnectionStateResponse> GetDeviceConnectionState(GrpcGetDeviceConnectionStateRequest request, ServerCallContext context)
    {
        var response = await _htmActorBridge.RequestHandlerActor
            .Ask<GetDeviceConnectionStateResponse>(
                new GetDeviceConnectionStateHtmRequest(request.DeviceType.ToDeviceType()));

        return new GrpcGetDeviceConnectionStateResponse
        {
            IsConnected = response.IsConnected
        };
    }

    public override async Task<GrpcGetMessageByCommandResponse> GetMessageByCommand(GrpcGetMessageByCommandRequest request, ServerCallContext context)
    {
        var response = await _htmActorBridge.RequestHandlerActor
            .Ask<GetMessageByCommandHtmResponse>(
                new GetMessageByCommandHtmRequest(Enum.Parse<SerialPortCommand>(request.Command)));

        return new GrpcGetMessageByCommandResponse
        {
          Message  = response.Message
        };
    }

    public override async Task<GrpcGetTemperatureMeasurementsResponse> GrpcGetTemperatureMeasurements(GrpcGetTemperatureMeasurementsRequest request, ServerCallContext context)
    {
        var from = request.From.ToDateTime();
        var to = request.To.ToDateTime();
        
        var response = await _htmActorBridge.RequestHandlerActor
            .Ask<GetTemperatureMeasurementsByDateRangeResponse>(
                new GetTemperatureMeasurementsByDateRangeHtmRequest(from, to));

        var grpcResponse = new GrpcGetTemperatureMeasurementsResponse();
        grpcResponse.GrpcTemperatureMeasurements.AddRange(
            response.TemperatureMeasurements?.Select(x => x.ToGrpcTemperatureMeasurement()));

        return grpcResponse;
    }
}