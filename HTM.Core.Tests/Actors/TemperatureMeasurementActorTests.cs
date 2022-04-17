using System.Threading.Tasks;
using Akka.Actor;
using Akka.TestKit.Xunit2;
using HTM.Core.Actors;
using HTM.Core.Adapters;
using HTM.Infrastructure.Measurements.Messages.Requests;
using HTM.Infrastructure.Models;
using Moq;
using Xunit;

namespace HTM.Core.Tests.Actors;

public class TemperatureMeasurementActorTests : TestKit
{
    [Fact]
    public void WhenReceiveGetLastTemperatureMeasurementRequestShouldReturnLastTemperatureMeasurement()
    {
        // Arrange
        const long measurementId = 1;
        
        var temperatureMeasurementDaoMock = new Mock<ITemperatureMeasurementDao>();
        temperatureMeasurementDaoMock
            .Setup(x => x.GetLastMeasurement())
            .Returns(Task.FromResult(new TemperatureMeasurement
            {
                Id = measurementId
            }));

        var temperatureMeasurementActor = ActorOf(Props.Create<TemperatureMeasurementActor>(temperatureMeasurementDaoMock.Object));

        // Act
        temperatureMeasurementActor.Tell(new GetLastTemperatureMeasurementRequest());

        // Assert
        ExpectMsg<GetLastTemperatureMeasurementResponse>(response => response.TemperatureMeasurement.Id == measurementId);
    }
}