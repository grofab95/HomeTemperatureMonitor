using HTM.Infrastructure.Devices.Enums;
using HTM.Web.Communication.Services;
using Microsoft.AspNetCore.Components;

namespace HTM.Web.Pages;

public partial class Index : IDisposable
{
    [Inject] public IHtmEventsService HtmEventsService { get; set; }

    private string _message;
    private bool _isConnected;
    
    protected override void OnInitialized()
    {
        HtmEventsService.OnDeviceConnectionChangedEvent += OnDeviceConnectionChangedEvent;
    }

    private void OnDeviceConnectionChangedEvent(object? sender, (DeviceType deviceType, bool isConnected) e)
    {
        InvokeAsync(() =>
        {
            _isConnected = e.isConnected;
            StateHasChanged();
        });
    }

    private void OnMessage(object? sender, string message)
    {
        InvokeAsync(() =>
        {
            _message = message;
            StateHasChanged();
        });
    }

    public void Dispose()
    {
        HtmEventsService.OnDeviceConnectionChangedEvent -= OnDeviceConnectionChangedEvent;
    }

    private void TurnOnLed()
    {
        //ArduinoBridge.SendMessage("1");
    }
    
    private void TurnOffLed()
    {
        //ArduinoBridge.SendMessage("0");
    }
}