using System;
using System.Threading.Tasks;
using HTM.Infrastructure;
using HTM.Infrastructure.Devices.Enums;
using HTM.Web.Communication.Services;
using Microsoft.AspNetCore.Components;

namespace HTM.Web.Pages;

public partial class Index : IDisposable
{
    [Inject] public IHtmEventsService HtmEventsService { get; set; }
    [Inject] public HtmMethodsClient HtmMethodsClient { get; set; }

    private string _message;
    private bool _isConnected;
    
    protected override async Task OnInitializedAsync()
    {
        HtmEventsService.OnDeviceConnectionChangedEvent += OnDeviceConnectionChangedEvent;

        _isConnected = await HtmMethodsClient.GetDeviceConnectionStatus(DeviceType.Arduino);
    }

    private void OnDeviceConnectionChangedEvent(object sender, (DeviceType deviceType, bool isConnected) e)
    {
        InvokeAsync(() =>
        {
            _isConnected = e.isConnected;
            StateHasChanged();
        });
    }

    private void OnMessage(object sender, string message)
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

    private async Task TurnOnLed()
    {
        await HtmMethodsClient.GetMessageByCommand(SerialPortCommand.TurnLedOn);
    }
    
    private async Task TurnOffLed()
    {
        await HtmMethodsClient.GetMessageByCommand(SerialPortCommand.TurnLedOff);
    }
}