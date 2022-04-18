using System;
using System.IO;
using HTM.Devices.Arduino.Configurations;
using HTM.Infrastructure.Devices.Adapters;
using HTM.Infrastructure.Exceptions;
using Microsoft.Extensions.Options;
using Serilog;
using System.IO.Ports;
using System.Linq;
using System.Threading.Tasks;
using HTM.Infrastructure.Models;

namespace HTM.Devices.Arduino.Services;

public class ArduinoService : ISerialPortDevice
{
    public event EventHandler<SerialPortMessage[]> OnMessagesReceived;
    public event EventHandler<bool> ConnectionChanged;

    private const int CheckConnectionHealthDelayInMs = 300;
    
    private readonly SerialPort _serialPort;
    private bool _isConnected;
    private bool _disconnectedLogged;

    public ArduinoService(IOptions<ArduinoOptions> arduinoOptions)
    {
        _serialPort = new SerialPort(arduinoOptions.Value.PortName, arduinoOptions.Value.PortBaudRate);
        _serialPort.DataReceived += OnDataReceived;
    }

    public async Task Initialize()
    {
        await Task.Factory.StartNew(CheckConnectionHealthLoop, TaskCreationOptions.LongRunning);
    }

    private async Task CheckConnectionHealthLoop()
    {
        while (true)
        {
            if (!_serialPort.IsOpen)
            {
                TryOpenPort();
            }

            if (_serialPort.IsOpen != _isConnected)
            {
                _isConnected = _serialPort.IsOpen;

                ConnectionChanged?.Invoke(this, _serialPort.IsOpen);
            }

            await Task.Delay(CheckConnectionHealthDelayInMs);
        }
    }
    
    private void TryOpenPort()
    {
        try
        {
            _serialPort.Open();
            _disconnectedLogged = false;
        }
        catch (FileNotFoundException)
        {
            if (!_disconnectedLogged)
            {
                Log.Error("{ArduinoService} | Arduino not found at {Port}", nameof(ArduinoService), _serialPort.PortName);
                _disconnectedLogged = true;
            }
        }
        catch (UnauthorizedAccessException)
        {
            Log.Error("{ArduinoService} | Access to {Port} is denied", nameof(ArduinoService), _serialPort.PortName);
        }
        catch
        {
            // ignored
        }
    }

    private void OnDataReceived(object sender, SerialDataReceivedEventArgs args)
    {
        var receivedMessage = _serialPort.ReadLine();
        if (string.IsNullOrEmpty(receivedMessage))
        {
            return;
        }

        var messages = ReadMessage(receivedMessage);
        if (messages == null)
        {
            return;
        }
        
        OnMessagesReceived?.Invoke(this, messages);
    }
    
    private SerialPortMessage[] ReadMessage(string receivedMessage)
    {
        try
        {
            var messages = receivedMessage.Split('|', StringSplitOptions.RemoveEmptyEntries);
            return messages
                .Select(m => m.Split('_'))
                .Where(x => x.Length == 2)
                .Select(x => new SerialPortMessage(Enum.Parse<SerialPortMessageType>(x[0].Trim()), x[1].Trim()))
                .ToArray();
        }
        catch (Exception ex)
        {
            Log.Error(ex, "{ArduinoService} | ReadMessage", nameof(ArduinoService));
            
            return null;
        }
    }

    public void SendMessage(string message)
    {
        if (string.IsNullOrEmpty(message))
        {
            throw new ArgumentNullException(message);
        }
        
        if (!_serialPort.IsOpen)
        {
            throw new DeviceDisconnectedException();
        }
        
        Log.Debug("{SendMessage} | Message={Message}", nameof(SendMessage), message);
        
        _serialPort.Write(message);
    }
    
    public void Dispose()
    {
        _serialPort.DataReceived -= OnDataReceived;
        _serialPort.Close();
        _serialPort.Dispose();
    }
}

    