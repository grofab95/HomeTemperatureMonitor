using Akka.Actor;
using HTM.Infrastructure.Akka;
using HTM.Infrastructure.MessagesBase;

namespace HTM.Core.Actors;

public class RequestHandlerActor : BaseActor
{
    private readonly Dictionary<Guid, IActorRef> _messageIdToActor = new();

    public RequestHandlerActor()
    {
        Receive<HtmRequest>(SaveRequestSenderAndPublishToEventStream);
        Receive<HtmResponse>(SendResponseToSenderAndRemoveFromDictionary);
    }

    private void SaveRequestSenderAndPublishToEventStream<T>(T request) where T : HtmRequest
    {
        Logger.Info("SaveRequestSenderAndPublishToEventStream | RequestId={RequestId}", request.RequestId);
        
        _messageIdToActor.Add(request.RequestId, Sender);
        
        Context.System.EventStream.Publish(request);
    }

    private void SendResponseToSenderAndRemoveFromDictionary<T>(T response) where T : HtmResponse
    {
        if (!_messageIdToActor.ContainsKey(response.RequestId))
        {
            Logger.Error("SendResponseToSenderAndRemoveFromDictionary | Sender not recognized, RequestId={RequestId}", response.RequestId);
            return;
        } 
        
        _messageIdToActor[response.RequestId].Tell(response);
        _messageIdToActor.Remove(response.RequestId);
    }
}