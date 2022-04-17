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

        Receive<AddTemperatureMeasurementHtmRequest>(AddMeasurement);
        Receive<GetLastTemperatureMeasurementHtmRequest>(GetLastMeasurement);
        Receive<GetTemperatureMeasurementsByDateRangeHtmRequest>(GetMeasurementsByDateRange);
        
        Context.System.EventStream.Subscribe<AddTemperatureMeasurementHtmRequest>(Self);
        Context.System.EventStream.Subscribe<GetLastTemperatureMeasurementHtmRequest>(Self);
        Context.System.EventStream.Subscribe<GetTemperatureMeasurementsByDateRangeHtmRequest>(Self);
        
        Context.System.EventStream.Publish(TemperatureMeasurementActorInitializedEvent.Instance);
    }
    
    private void AddMeasurement(AddTemperatureMeasurementHtmRequest request)
    {
        Logger.Info("AddMeasurement");

        _temperatureMeasurementDao.AddMeasurement(request.TemperatureMeasurement)
            .PipeTo(
                Sender, 
                Self, 
                () => AddTemperatureMeasurementResponse.WithSuccess(request.RequestId),
                ex => AddTemperatureMeasurementResponse.WithFailure(request.RequestId, ex));
    }

    private void GetLastMeasurement(GetLastTemperatureMeasurementHtmRequest request)
    {
        Logger.Info("GetLastMeasurement");
        
        _temperatureMeasurementDao.GetLastMeasurement()
            .PipeTo(
                Sender, 
                Self, 
                data => GetLastTemperatureMeasurementResponse.WithSuccess(request.RequestId, data),
                ex => GetLastTemperatureMeasurementResponse.WithFailure(request.RequestId, ex));
    }

    private void GetMeasurementsByDateRange(GetTemperatureMeasurementsByDateRangeHtmRequest request)
    {
        Logger.Info("GetMeasurementsByDateRange | From={From}, To={To}", request.From, request.To);
        
        _temperatureMeasurementDao.GetMeasurementsByDateRange(request.From, request.To)
            .PipeTo(
                Sender, 
                Self, 
                data => GetTemperatureMeasurementsByDateRangeResponse.WithSuccess(request.RequestId, data),
                ex => GetTemperatureMeasurementsByDateRangeResponse.WithFailure(request.RequestId, ex));
    }
}