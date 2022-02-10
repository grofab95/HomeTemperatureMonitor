using HTM.Infrastructure.Models;

namespace HTM.Infrastructure.Adapters;

public interface ISensorDevice 
{
    Task<Temperature> GetTemperature();
}