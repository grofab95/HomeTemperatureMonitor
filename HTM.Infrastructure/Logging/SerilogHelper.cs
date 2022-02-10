using Serilog;
using Serilog.Events;

namespace HTM.Infrastructure.Logging;

public static class SerilogHelper
{
    public static void AddSerilog()
    {
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Information()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
            .Enrich.FromLogContext()
            .WriteTo.Console(outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level:u4}][{LogSource}] {Message:lj}{NewLine}{Exception}")
            .WriteTo.File(
                "logs//LOG_.log",
                outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level:u4}][{LogSource}] {Message:lj}{NewLine}{Exception}",
                restrictedToMinimumLevel: LogEventLevel.Information,
                rollingInterval: RollingInterval.Day)
            .CreateBootstrapLogger();
    }
}