using Grpc.Core;
using HTM.Communication.V1;

namespace HTM.Communication.Services;

public class HtmMethodsServer : HTMMethodsService.HTMMethodsServiceBase
{
    public override Task<GetMessageByCommandResponse> GetMessageByCommand(GetMessageByCommandRequest request, ServerCallContext context)
    {
        return new Task<GetMessageByCommandResponse>(null);
    }
}