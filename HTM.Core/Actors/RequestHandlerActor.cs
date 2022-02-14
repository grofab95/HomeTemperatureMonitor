using Akka.Actor;
using HTM.Infrastructure.Akka;
using HTM.Infrastructure.Devices.Messages.Requests;

namespace HTM.Core.Actors;

public class RequestHandlerActor : BaseActor
{
    private IActorRef _sender; // todo: create query actor
    
    public RequestHandlerActor()
    {
        Receive<GetDeviceConnectionStateRequest>(r =>
        {
            _sender = Sender;
            Context.System.EventStream.Publish(r);
        });

        Receive<GetDeviceConnectionStateResponse>(res =>
        {
            
            _sender.Tell(res);
        });
        
        Receive<GetMessageByCommandRequest>(r =>
        {
            _sender = Sender;
            Context.System.EventStream.Publish(r);
        });

        Receive<GetMessageByCommandResponse>(res =>
        {
            
            _sender.Tell(res);
        });
    }
}