namespace HTM.Infrastructure.Models;

public class SerialPortMessage
{
    public SerialPortMessageType Type { get; }
    public string Text { get; }

    public SerialPortMessage(SerialPortMessageType type, string text)
    {
        Type = type;
        Text = text;
    }
}