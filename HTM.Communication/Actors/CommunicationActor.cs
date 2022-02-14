using Akka.Event;
using Grpc.Net.Client;
using HTM.Communication.Extensions;
using HTM.Communication.V1;
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
        
        Context.System.EventStream.Subscribe<DeviceConnectionChangedEvent>(Self);
    }

    public async Task CallDeviceConnectionChanged(DeviceConnectionChangedEvent @event)
    {
        try
        {
            var client = new HTMEventsService.HTMEventsServiceClient(_grpcChannel);

            var response = await client.DeviceConnectionChangedAsync(new DeviceConnectionChangedRequest
            {
                DeviceType = @event.DeviceType.ToDeviceType(), IsConnected = @event.IsConnected
            });
        }
        catch (Exception ex)
        {
            //todo:
        }
    }
}