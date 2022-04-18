using Grpc.Core;
using HTM.Communication.V2;
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
    
    public override Task<GrpcDeviceConnectionChangedResponse> DeviceConnectionChanged(GrpcDeviceConnectionChangedRequest request, ServerCallContext context)
    {
        Log.Information("{HtmEventsServer} - {DeviceConnectionChanged} | Request={Request}", 
            nameof(HtmEventsServer), nameof(DeviceConnectionChanged), JsonConvert.SerializeObject(request));
        
        _htmEventsInvoker.InvokeDeviceConnectionChangedEvent(request.DeviceType.ToDeviceType(), request.IsConnected);
        
        return Task.FromResult(new GrpcDeviceConnectionChangedResponse());
    }

    public override Task<GrpcSerialPortMessagesReceivedResponse> SerialPortMessagesReceived(GrpcSerialPortMessagesReceivedRequest request, ServerCallContext context)
    {
        Log.Information("{HtmEventsServer} - {SerialPortMessagesReceived} | Request={Request}", 
            nameof(HtmEventsServer), nameof(SerialPortMessagesReceived), JsonConvert.SerializeObject(request));

        var messages = request.Messages.Select(x => x.ToSerialPortMessage()).ToArray();
        _htmEventsInvoker.InvokeSerialPortMessagesReceivedEvent(messages);
        
        return Task.FromResult(new GrpcSerialPortMessagesReceivedResponse());
    }
}

