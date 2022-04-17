using HTM.Infrastructure;
using HTM.Infrastructure.Akka;
using HTM.Infrastructure.Measurements.Messages.Requests;
using HTM.Infrastructure.Messages.Events;
using HTM.Infrastructure.Models;
using System.Globalization;
using Akka.Event;
using HTM.Infrastructure.Devices.Messages.Requests;

namespace HTM.Core.Actors;

public class TemperatureMonitorActor : BaseActor
{
    private readonly TimeSpan _loopDelay = TimeSpan.FromSeconds(10);
    private float? _lastTemperature;
    public TemperatureMonitorActor()
    {
        Receive<TemperatureMeasurementActorInitializedEvent>(_ =>
        {
            Context.System.EventStream.Publish(new GetLastTemperatureMeasurementHtmRequest());
        });
        
        Receive<GetLastTemperatureMeasurementResponse>(response =>
        {
            _lastTemperature = response.TemperatureMeasurement?.Temperature;
            Context.System.Scheduler.ScheduleTellRepeatedly(TimeSpan.Zero, _loopDelay, Self, TimerElapsedEvent.Instance, Self);
        });
        
        Receive<TimerElapsedEvent>(_ =>
        {
            Context.System.EventStream.Publish(new GetMessageByCommandHtmRequest(SerialPortCommand.GetTemperature));
        });

        Receive<GetMessageByCommandHtmResponse>(OnGetMessageByCommandResponse);
        Receive<AddTemperatureMeasurementResponse>(r => { });
        
        Context.System.EventStream.Subscribe<GetLastTemperatureMeasurementResponse>(Self);
        Context.System.EventStream.Subscribe<TemperatureMeasurementActorInitializedEvent>(Self);
    }
    private void OnGetMessageByCommandResponse(GetMessageByCommandHtmResponse htmResponse)
    {
        if (htmResponse.IsError)
        {
            return;
        }

        // if (!float.TryParse(response?.Message, out float temperature, CultureInfo.InvariantCulture))
        // {
        //     return; // InvalidTemperatureException
        // }

        var temperature = (float)Math.Round(float.Parse(htmResponse.Message ?? "9999", CultureInfo.InvariantCulture), 2);

        if (temperature == _lastTemperature)
        {
            return;
        }

        _lastTemperature = temperature;
        
        Context.System.EventStream.Publish(new AddTemperatureMeasurementHtmRequest(new TemperatureMeasurement
        {
            MeasurementDate = DateTime.Now,
            Temperature = temperature
        }));
    }
}