using System;
using System.Threading.Tasks;
using HTM.Infrastructure;
using HTM.Infrastructure.Devices.Enums;
using HTM.Web.Communication.Services;
using HTM.Web.Shared;
using Microsoft.AspNetCore.Components;

namespace HTM.Web.Pages;

public partial class ArduinoDiagnosticPage : IDisposable
{
    [Inject] public IHtmEventsService HtmEventsService { get; set; }
    [Inject] public HtmMethodsClient HtmMethodsClient { get; set; }

    private PleaseWaitComponent _pleaseWaitComponent;
    private string _errorMessage;
    private bool _isConnected;
    private bool _isLedOn;
    
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (!firstRender)
        {
            return;
        }
        
        _pleaseWaitComponent.Show();
        
        HtmEventsService.OnDeviceConnectionChangedEvent += OnDeviceConnectionChangedEvent;
    
        _isConnected = await HtmMethodsClient.GetDeviceConnectionStatus(DeviceType.Arduino);
    
        await GetLedState();
        
        _pleaseWaitComponent.Close();
        
        StateHasChanged();
    }

    private async Task GetLedState()
    {
        await HandleOperation(async () =>
        {
            var stateString = await HtmMethodsClient.GetMessageByCommand(SerialPortCommand.GetLedState);

            _isLedOn = stateString.Replace("\r", "") == "1";
        });
    }

    private void OnDeviceConnectionChangedEvent(object sender, (DeviceType deviceType, bool isConnected) e)
    {
        InvokeAsync(() =>
        {
            _errorMessage = null;
            _isConnected = e.isConnected;
            StateHasChanged();
        });
    }

    private void OnMessage(object sender, string message)
    {
        InvokeAsync(() =>
        {
            _errorMessage = message;
            StateHasChanged();
        });
    }

    private async Task SwitchLed()
    {
        
        await HandleOperation(async () =>
        {
            var command = _isLedOn ? SerialPortCommand.TurnLedOff : SerialPortCommand.TurnLedOn;
            await HtmMethodsClient.GetMessageByCommand(command);
            
            _isLedOn = !_isLedOn;
        });
    }

    private async Task HandleOperation(Func<Task> operation)
    {
        try
        {
            _errorMessage = null;
            await operation();
        }
        catch (Exception ex)
        {
            _errorMessage = ex.Message;
        }
    }
    
    public void Dispose()
    {
        HtmEventsService.OnDeviceConnectionChangedEvent -= OnDeviceConnectionChangedEvent;
    }
}