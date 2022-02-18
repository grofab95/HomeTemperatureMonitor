using System.IO.Ports;
using HTM.Devices.Arduino.Configurations;
using HTM.Infrastructure.Devices.Enums;
using HTM.Infrastructure.Exceptions;
using Serilog;

namespace HTM.Devices.Arduino.Services;

public class ArduinoManager
{
    private readonly ArduinoOptions _arduinoOptions;
    private readonly string[] _comPorts = Enumerable.Range(1, 256).Select((x => $"COM{x}")).ToArray();
    
    public ArduinoManager(ArduinoOptions arduinoOptions)
    {
        _arduinoOptions = arduinoOptions;
    }

    public SerialPort GetArduino()
    {
        return GetArduino(new SerialPort(_arduinoOptions.PortName, _arduinoOptions.PortBaudRate));
    }
    
    public SerialPort GetArduino(SerialPort arduino)
    {
        if (_arduinoOptions.PortName != ArduinoOptions.Auto)
        {
            arduino.PortName = _arduinoOptions.PortName;
        }
        
        foreach (var comPort in _comPorts)
        {
            if (IsArduinoAvailable(arduino, comPort))
            {
                return arduino;
            }
        }

        throw new DeviceNotFoundException(DeviceType.Arduino);
    }

    private bool IsArduinoAvailable(SerialPort arduino, string comPort)
    {
        try
        {
            Log.Debug("{ArduinoFinder} | Checking {ComPort}", nameof(ArduinoManager), comPort);

            arduino.PortName = comPort;
            
            arduino.Open();
            arduino.Close();
            
            Log.Debug("{ArduinoFinder} | Found arduino at {ComPort}", nameof(ArduinoManager), comPort);

            return true;
        }
        catch (Exception e)
        {
            return false;
        }
    }
}