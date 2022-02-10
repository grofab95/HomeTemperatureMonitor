using Akka.Actor;
using Akka.Event;

namespace HTM.Core.Actors;

public abstract class BaseActor : ReceiveActor
{
    protected readonly ILoggingAdapter Logger;

    protected BaseActor()
    {
        Logger = Context.GetLogger();
    }

    protected override void PreStart() => Logger.Info("Started");

    protected override void PostStop() => Logger.Info("Stopped");
}