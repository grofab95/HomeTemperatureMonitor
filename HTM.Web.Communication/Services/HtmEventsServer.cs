using Grpc.Core;
using HTM.Communication.V1;
using HTM.Web.Communication.Extensions;
using Newtonsoft.Json;
using Serilog;

namespace HTM.Web.Communication.Services;

public class HtmEventsServer : HTMEventsService.HTMEventsServiceBase
{
    private readonly IHtmEventsInvoker _htmEventsInvoker;

    public HtmEventsServer(IHtmEventsInvoker htmEventsInvoker)
    {
        _htmEventsInvoker = htmEventsInvoker;
    }
    
    public override Task<DeviceConnectionChangedResponse> DeviceConnectionChanged(DeviceConnectionChangedRequest request, ServerCallContext context)
    {
        Log.Information("{HtmEventsServer} - {DeviceConnectionChanged} | Request={Request}", 
            nameof(HtmEventsServer), nameof(DeviceConnectionChanged), JsonConvert.SerializeObject(request));
        
        _htmEventsInvoker.InvokeDeviceConnectionChangedEvent(request.DeviceType.ToDeviceType(), request.IsConnected);
        
        return Task.FromResult(new DeviceConnectionChangedResponse());
    }
}

