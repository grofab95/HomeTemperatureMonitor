using Akka.Event;
using Grpc.Net.Client;
using HTM.Communication.Extensions;
using HTM.Communication.V2;
using HTM.Infrastructure.Akka;
using HTM.Infrastructure.Devices.Messages.Events;

namespace HTM.Communication.Actors;

public class CommunicationActor : BaseActor
{
    private readonly GrpcChannel _grpcChannel;
    
    public CommunicationActor()
    {
        _grpcChannel = GrpcChannel.ForAddress("http://localhost:2005");
        
        ReceiveAsync<DeviceConnectionChangedEvent>(CallDeviceConnectionChanged);
        ReceiveAsync<SerialPortMessageReceivedEvent>(CallMessageReceivedEvent);
        
        Context.System.EventStream.Subscribe<DeviceConnectionChangedEvent>(Self);
        Context.System.EventStream.Subscribe<SerialPortMessageReceivedEvent>(Self);
    }

    private async Task CallDeviceConnectionChanged(DeviceConnectionChangedEvent @event)
    {
        try
        {
            var client = new HTMEventsService.HTMEventsServiceClient(_grpcChannel);

            var response = await client.DeviceConnectionChangedAsync(new GrpcDeviceConnectionChangedRequest
            {
                DeviceType = @event.DeviceType.ToDeviceType(), IsConnected = @event.IsConnected
            });
        }
        catch (Exception ex)
        {
            Logger.Error(ex, "CallDeviceConnectionChanged Error");
        }
    }
    
    private async Task CallMessageReceivedEvent(SerialPortMessageReceivedEvent @event)
    {
        try
        {
            var client = new HTMEventsService.HTMEventsServiceClient(_grpcChannel);

            var request = new GrpcSerialPortMessagesReceivedRequest();
            
            request.Messages.AddRange(@event.Messages.Select(x => x.ToSerialPortMessage()));
            
            var response = await client.SerialPortMessagesReceivedAsync(request);
        }
        catch (Exception ex)
        {
            Logger.Error(ex, "CallMessageReceivedEvent Error");
        }
    }
}