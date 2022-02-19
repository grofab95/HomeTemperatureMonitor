using HTM.Devices.Arduino.Configurations;
using HTM.Devices.Arduino.Services;
using HTM.Infrastructure.Constants;
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
        
        var portName = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile(Application.AppSettingsFile)
            .Build()
            .GetSection(ArduinoOptions.SectionName)[nameof(ArduinoOptions.PortName)];

        if (portName == ArduinoOptions.Emulator)
        {
            services.AddSingleton<ISerialPortDevice, ArduinoEmulatorService>();
        }
        else
        {
            services.AddSingleton<ISerialPortDevice, ArduinoService>();
        }
    }
}