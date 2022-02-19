using Grpc.Core;
using HTM.Communication.Services;
using HTM.Communication.V1;
using HTM.Core.Services;
using HTM.Database;
using HTM.Database.Extensions;
using HTM.Devices.Arduino.Extensions;
using HTM.Infrastructure.Adapters;
using HTM.Infrastructure.Akka;
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
            services.AddDatabase();
            services.AddSingleton<HtmActorBridge>();
            services.AddSingleton<IPersistenceInitializer, DatabaseInitializer>();
            services.AddHostedService<AppService>();
            services.AddSingleton(sp =>
            {
                var htmActorBridge = sp.GetRequiredService<HtmActorBridge>();

                return new Server
                {
                    Services = {HTMMethodsService.BindService(new HtmMethodsServer(htmActorBridge))},
                    Ports = {new ServerPort("localhost", 2010, ServerCredentials.Insecure)}
                };
            });

            services.AddHostedService<GrpcHostedService>();
        });