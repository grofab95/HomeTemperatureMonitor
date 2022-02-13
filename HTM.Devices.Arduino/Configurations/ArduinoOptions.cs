﻿namespace HTM.Devices.Arduino.Configurations;

public class ArduinoOptions
{
    public const string SectionName = "Arduino";

    public string PortName { get; set; } = "COM4";
    public int PortBaudRate { get; set; } = 9600;
}