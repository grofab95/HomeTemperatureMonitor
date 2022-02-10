using HTM.Core.Actors;
using HTM.Infrastructure.Devices.Enums;
using HTM.Infrastructure.Devices.Messages.Events;
using HTM.Infrastructure.Devices.Messages.Requests;
using Akka.Event;

namespace HTM.Core.Devices.Arduino.Actors;

public class TestActor : BaseActor
{
    public TestActor()
    {
        Receive<TestMessage>(m =>
        {
            
            Context.System.EventStream.Publish(new GetTemperatureResponse(m.Message));
        });
    }
}