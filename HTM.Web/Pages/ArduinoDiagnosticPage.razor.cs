using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HTM.Infrastructure.Devices.Enums;
using HTM.Infrastructure.Models;
using HTM.Web.Communication.Services;
using HTM.Web.Shared;
using Microsoft.AspNetCore.Components;

namespace HTM.Web.Pages;

record MessageInfo(DateTime ReceivedAt, SerialPortMessageType Type, string Text);

public partial class ArduinoDiagnosticPage : IDisposable
{
    [Inject] public IHtmEventsService HtmEventsService { get; set; }
    [Inject] public HtmMethodsClient HtmMethodsClient { get; set; }

    private PleaseWaitComponent _pleaseWaitComponent;
    private string _errorMessage;
    private bool _isConnected;
    private bool _isLedOn;

    private readonly List<MessageInfo> _receivedMessages = new();
    
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (!firstRender)
        {
            return;
        }
        
        _pleaseWaitComponent.Show();
        
        HtmEventsService.OnDeviceConnectionChangedEvent += OnDeviceConnectionChangedEvent;
        HtmEventsService.OnSerialPortMessagesReceivedEvent += OnMessagesReceived;
    
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

    private void OnMessagesReceived(object sender, SerialPortMessage[] messages)
    {
        InvokeAsync(() =>
        {
            _receivedMessages.AddRange(messages.Select(x => new MessageInfo(DateTime.Now, x.Type, x.Text)));
            
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
        HtmEventsService.OnSerialPortMessagesReceivedEvent -= OnMessagesReceived;
    }
}