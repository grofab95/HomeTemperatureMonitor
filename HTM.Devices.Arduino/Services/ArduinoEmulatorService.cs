using System;
using System.Threading;
using System.Threading.Tasks;
using HTM.Infrastructure.Devices.Adapters;
using HTM.Infrastructure.Models;

namespace HTM.Devices.Arduino.Services;

public class ArduinoEmulatorService : ISerialPortDevice
{
    public event EventHandler<bool> ConnectionChanged;
    public event EventHandler<SerialPortMessage[]> OnMessagesReceived;

    private readonly Random _random = new Random();

    public async Task Initialize()
    {
        await Task.Delay(TimeSpan.FromSeconds(5));
        
        ConnectionChanged?.Invoke(this, true);
    }

    public void SendMessage(string message)
    {
        if (!Enum.TryParse<SerialPortCommand>(message, out var command))
        {
            return;
        }
        
        Thread.Sleep(1500);

        var callbackMessage = command switch
        {
            SerialPortCommand.TurnLedOn or SerialPortCommand.TurnLedOff => "ok",
            SerialPortCommand.GetTemperature => _random.Next(19, 27).ToString(),
            
            _ => string.Empty
        };
        
        OnMessagesReceived?.Invoke(this, new SerialPortMessage[] { new SerialPortMessage(SerialPortMessageType.CommandResponse, callbackMessage) });
    }

    public void Dispose()
    {
    }
}