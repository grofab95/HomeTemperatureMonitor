using HTM.Devices.Arduino.Configurations;
using HTM.Infrastructure.Devices.Adapters;
using HTM.Infrastructure.Exceptions;
using Microsoft.Extensions.Options;
using Serilog;
using System.IO.Ports;
using Serilog.Core;
using Timer = System.Timers.Timer;

namespace HTM.Devices.Arduino.Services;

public class ArduinoService : IDisposable, ISerialPortDevice
{
    public event EventHandler<string>? OnMessageReceived;
    public event EventHandler<bool>? ConnectionChanged;

    private const int CheckConnectionHealthDelayInMs = 300;

    private readonly ArduinoManager _arduinoManager;
    private readonly SerialPort _serialPort;
    private readonly Timer _timer;
    private bool _isConnected;

    public ArduinoService(IOptions<ArduinoOptions> arduinoOptions)
    {
        _arduinoManager = new ArduinoManager(arduinoOptions.Value);

        _serialPort = _arduinoManager.GetArduino();
        _serialPort.DataReceived += OnDataReceived;
        _timer = new Timer(CheckConnectionHealthDelayInMs);
        _timer.Elapsed += (_,_) => CheckConnectionHealth();
        _timer.Start();
    }

    private void CheckConnectionHealth()
    {
        try
        {
            if (!_serialPort.IsOpen)
            {
                _arduinoManager.GetArduino(_serialPort);
            }

            if (_serialPort.IsOpen == _isConnected)
            {
                return;
            }

            _isConnected = _serialPort.IsOpen;

            ConnectionChanged?.Invoke(this, _serialPort.IsOpen);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "CheckConnectionHealth");
        }
    }


    // private void TryOpenPort()
    // {
    //     try
    //     {
    //         _serialPort.Open();
    //     }
    //     catch (FileNotFoundException ex)
    //     {
    //         Log.Error("{ArduinoService} | Arduino not found at {Port}", nameof(ArduinoService), _serialPort.PortName);
    //     }
    //     catch (Exception ex)
    //     {
    //         // ignored
    //     }
    // }

    private void OnDataReceived(object sender, SerialDataReceivedEventArgs args)
    {
        var message = _serialPort.ReadLine();
        if (string.IsNullOrEmpty(message))
        {
            return;
        }
        
        OnMessageReceived?.Invoke(this, message);
    }
    
    public Task Initialize() => Task.CompletedTask;

    public void SendMessage(string? message)
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
        _timer.Close();
        _timer.Dispose();
        _serialPort.DataReceived -= OnDataReceived;
        _serialPort.Close();
        _serialPort.Dispose();
    }
}

    