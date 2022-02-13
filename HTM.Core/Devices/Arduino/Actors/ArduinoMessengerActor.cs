using Akka.Actor;
using Akka.Event;
using HTM.Core.Actors;
using HTM.Core.Devices.Arduino.Messages.Requests;
using HTM.Infrastructure.Devices.Messages.Events;
using HTM.Infrastructure.Messages.Events;

namespace HTM.Core.Devices.Arduino.Actors;

record RequestData(IActorRef Sender, GetMessageByCommandRequest Request);

public class ArduinoMessengerActor : BaseActor
{
    private readonly TimeSpan _timerDelay = TimeSpan.FromMilliseconds(500);
    
    private readonly List<RequestData?> _requests;
    private bool _isArduinoConnected;
    private DateTime _arduinoConnectedAt;

    private RequestData? _currentRequestData;

    public ArduinoMessengerActor()
    {
        _requests = new List<RequestData?>();

        Receive<GetMessageByCommandRequest>(request =>
        {
            Logger.Info("{ArduinoMessengerActor} Adding RequestId={RequestId}", nameof(ArduinoMessengerActor), request.RequestId);
            _requests.Add(new RequestData(Sender, request));
        });

        Receive<GetMessageByCommandResponse>(FinishHandleRequest);
        Receive<TimerElapsedEvent>(_ => StartHandleRequest());

        Receive<DeviceConnectionChangedEvent>(e =>
        {
            _arduinoConnectedAt = DateTime.Now;
            _isArduinoConnected = e.IsConnected;
        });
        
        Context.System.EventStream.Subscribe<GetMessageByCommandRequest>(Self);
        Context.System.EventStream.Subscribe<DeviceConnectionChangedEvent>(Self);
        
        Context.System.Scheduler.ScheduleTellRepeatedly(TimeSpan.Zero, _timerDelay, Self, TimerElapsedEvent.Instance, Self);
    }

    private void StartHandleRequest()
    {
        if (!_isArduinoConnected || !_requests.Any())
        {
            return;
        }

        if (DateTime.Now - _arduinoConnectedAt <= TimeSpan.FromSeconds(5))
        {
            return;
        }
        
        if (!Context.Child(nameof(GetMessageByCommandActor)).IsNobody())
        {
            return;
        }

        _currentRequestData = _requests.First();
        
        Logger.Info("{StartHandleRequest} | RequestId={RequestId}, Command={Command}", 
            nameof(StartHandleRequest), _currentRequestData?.Request.RequestId, _currentRequestData?.Request.Command);

        Context.ActorOf(Props.Create<GetMessageByCommandActor>(_currentRequestData?.Request), nameof(GetMessageByCommandActor));
    }

    private void FinishHandleRequest(GetMessageByCommandResponse response)
    {
        if (response.IsError)
        {
            Logger.Error("{FinishHandleRequest} | RequestId={RequestId}, Error={Error}", 
                nameof(FinishHandleRequest), _currentRequestData?.Request.RequestId, response.Exception?.Message);
        }
        else
        {
            Logger.Info("{FinishHandleRequest} | RequestId={RequestId}, Message={Message}",
                nameof(FinishHandleRequest), _currentRequestData?.Request.RequestId, response.Message);
            
            _requests.Remove(_currentRequestData);

        }
        
        _currentRequestData?.Sender.Tell(response);
    }
}