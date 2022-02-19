using Grpc.Core;
using HTM.Communication.Services;
using HTM.Communication.V1;
using HTM.Core.Services;
using HTM.Database;
using HTM.Database.Extensions;
using HTM.Devices.Arduino.Extensions;
using HTM.Infrastructure.Akka;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace HTM;

public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddArduino();
        services.AddDatabase();
        services.AddSingleton<HtmActorBridge>();
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
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        DatabaseInitializer.Initialize(app.ApplicationServices);
    }
}