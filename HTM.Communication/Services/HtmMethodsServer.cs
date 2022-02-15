using Akka.Actor;
using Grpc.Core;
using HTM.Communication.Extensions;
using HTM.Communication.V1;
using HTM.Infrastructure;
using HTM.Infrastructure.Akka;
using HTM.Infrastructure.Devices.Messages.Requests;

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
        var res = await _htmActorBridge.RequestHandlerActor
            .Ask<GetDeviceConnectionStateResponse>(
                new GetDeviceConnectionStateRequest(request.DeviceType.ToDeviceType()));

        return new GrpcGetDeviceConnectionStateResponse
        {
            IsConnected = res.IsConnected
        };
    }

    public override async Task<GrpcGetMessageByCommandResponse> GetMessageByCommand(GrpcGetMessageByCommandRequest request, ServerCallContext context)
    {
        var res = await _htmActorBridge.RequestHandlerActor
            .Ask<GetMessageByCommandResponse>(
                new GetMessageByCommandRequest(Enum.Parse<SerialPortCommand>(request.Command)));

        return new GrpcGetMessageByCommandResponse
        {
          Message  = res.Message
        };
    }
}