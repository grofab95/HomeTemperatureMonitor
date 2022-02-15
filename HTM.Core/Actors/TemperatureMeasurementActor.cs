using Akka.Actor;
using Akka.Event;
using HTM.Core.Adapters;
using HTM.Infrastructure.Akka;
using HTM.Infrastructure.Measurements.Messages.Requests;
using HTM.Infrastructure.Messages.Events;

namespace HTM.Core.Actors;

public class TemperatureMeasurementActor : BaseActor
{
    private readonly ITemperatureMeasurementDao _temperatureMeasurementDao;

    public TemperatureMeasurementActor(ITemperatureMeasurementDao temperatureMeasurementDao)
    {
        _temperatureMeasurementDao = temperatureMeasurementDao;

        Receive<AddTemperatureMeasurementRequest>(AddMeasurement);
        Receive<GetLastTemperatureMeasurementRequest>(GetLastMeasurement);
        Receive<GetTemperatureMeasurementsByDateRangeRequest>(GetMeasurementsByDateRange);
        
        Context.System.EventStream.Subscribe<AddTemperatureMeasurementRequest>(Self);
        Context.System.EventStream.Subscribe<GetLastTemperatureMeasurementRequest>(Self);
        Context.System.EventStream.Subscribe<GetTemperatureMeasurementsByDateRangeRequest>(Self);
        
        Context.System.EventStream.Publish(TemperatureMeasurementActorInitializedEvent.Instance);
    }
    
    private void AddMeasurement(AddTemperatureMeasurementRequest request)
    {
        Logger.Info("AddMeasurement");

        _temperatureMeasurementDao.AddMeasurement(request.TemperatureMeasurement)
            .PipeTo(
                Sender, 
                Self, 
                AddTemperatureMeasurementResponse.WithSuccess,
                AddTemperatureMeasurementResponse.WithFailure);
    }

    private void GetLastMeasurement(GetLastTemperatureMeasurementRequest _)
    {
        Logger.Info("GetLastMeasurement");
        
        _temperatureMeasurementDao.GetLastMeasurement()
            .PipeTo(
                Sender, 
                Self, 
                GetLastTemperatureMeasurementResponse.WithSuccess,
                GetLastTemperatureMeasurementResponse.WithFailure);
    }

    private void GetMeasurementsByDateRange(GetTemperatureMeasurementsByDateRangeRequest request)
    {
        Logger.Info("GetMeasurementsByDateRange | From={From}, To={To}", request.From, request.To);
        
        _temperatureMeasurementDao.GetMeasurementsByDateRange(request.From, request.To)
            .PipeTo(
                Sender, 
                Self, 
                GetTemperatureMeasurementsByDateRangeResponse.WithSuccess,
                GetTemperatureMeasurementsByDateRangeResponse.WithFailure);
    }
}