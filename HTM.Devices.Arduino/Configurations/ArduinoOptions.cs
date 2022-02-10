namespace HTM.Devices.Arduino.Configurations;

public class ArduinoOptions
{
    public const string SectionName = "Arduino";
    
    public string PortName { get; set; }
    public int PortBaudRate { get; set; }
}