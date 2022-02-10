using Akka.Actor;
using Akka.Configuration;
using Akka.DependencyInjection;
using HTM.Core.Actors;
using Microsoft.Extensions.Hosting;

namespace HTM.Core.Services;

public class AppService : IHostedService
{
    private readonly IServiceProvider _serviceProvider;
    private ActorSystem? _actorSystem;

    public AppService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }
    
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        var configuration = await File.ReadAllTextAsync(Path.Combine(Directory.GetCurrentDirectory(), "akkaConfig.json"), cancellationToken);
        var akkaConfig = ConfigurationFactory.ParseString(configuration);

        var actorSystemSetup = BootstrapSetup
            .Create()
            .WithConfig(akkaConfig)
            .And(DependencyResolverSetup.Create(_serviceProvider));
        
        _actorSystem = ActorSystem.Create("HtmActorSystem", actorSystemSetup);
        _actorSystem.ActorOf(DependencyResolver.For(_actorSystem).Props<HtmActor>(), nameof(HtmActor));
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _actorSystem?.Terminate();
        return Task.CompletedTask;
    }
}