using Akka.Actor;

namespace HTM.Infrastructure.Akka;

public class HtmActorBridge
{
    public IActorRef RequestHandlerActor { get; private set; }

    public void AddRequestHandlerActor(IActorRef actor)
    {
        RequestHandlerActor = actor;
    }
}