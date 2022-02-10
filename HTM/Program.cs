using HTM.Core.Services;
using HTM.Devices.Arduino.Extensions;
using HTM.Infrastructure.Logging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;

SerilogHelper.AddSerilog();

CreateHostBuilder(args).Build().Run();

static IHostBuilder CreateHostBuilder(string[] args) => 
    Host.CreateDefaultBuilder(args)
        .ConfigureLogging(logging =>
        {
            logging.ClearProviders();
            logging.AddSerilog();
        })
        .ConfigureServices(services =>
        {
            services.AddArduino();
            services.AddHostedService<AppService>();
        });