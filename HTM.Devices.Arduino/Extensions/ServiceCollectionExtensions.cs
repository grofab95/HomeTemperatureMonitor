using HTM.Devices.Arduino.Configurations;
using HTM.Devices.Arduino.Services;
using HTM.Infrastructure.Devices.Adapters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HTM.Devices.Arduino.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddArduino(this IServiceCollection services)
    {
        services.AddOptions<ArduinoOptions>()
            .Configure<IConfiguration>((o, c) => c.GetSection(ArduinoOptions.SectionName).Bind(o));

        services.AddSingleton<ISerialPortDevice, ArduinoService>();
    }
}