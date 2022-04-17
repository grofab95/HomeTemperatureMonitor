using System;
using Akka.Actor;
using Akka.TestKit;
using Akka.TestKit.Xunit2;
using HTM.Core.Actors;
using HTM.Infrastructure.Devices.Messages.Requests;
using Xunit;

namespace HTM.Core.Tests.Actors;

public class RequestHandlerActorTests : TestKit
{
    [Fact]
    public void WhenReceiveRequestShouldSaveSenderId()
    {
        // Arrange
        var requestHandlerActor = ActorOf(Props.Create<RequestHandlerActor>());
        var request = new SendMessageRequest("test");

        var subscriber = CreateTestProbe("subscriber");
        Sys.EventStream.Subscribe(subscriber, typeof(SendMessageRequest));
        
        subscriber.SetAutoPilot(new DelegateAutoPilot((sender, message) =>
        {
            var requestId = (message as SendMessageRequest)?.RequestId ?? Guid.Empty;
            
            sender.Tell(new SendMessageResponse(requestId));
            
            return AutoPilot.KeepRunning;
        }));

        // Act
        requestHandlerActor.Tell(request);

        // Assert
        ExpectMsg<SendMessageResponse>(response => response.RequestId == request.RequestId);
    }
}