namespace HTM.Infrastructure.Devices.Messages.Requests;

public class GetTemperatureResponse
{
    public string Temperature { get; }
    
    public GetTemperatureResponse(string temperature)
    {
        Temperature = temperature;
    }
}