using Akka.Actor;
using Akka.Configuration;
using Akka.DependencyInjection;
using HTM.Core.Actors;
using HTM.Infrastructure.Adapters;
using HTM.Infrastructure.Akka;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace HTM.Core.Services;

public class AppService : IHostedService
{
    private readonly IServiceProvider _serviceProvider;
    private ActorSystem _actorSystem;

    public AppService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        var persistenceInitializer = _serviceProvider.GetRequiredService<IPersistenceInitializer>();
        await persistenceInitializer.Initialize();
        
        var configuration = await File.ReadAllTextAsync(
            Path.Combine(Directory.GetCurrentDirectory(), "akkaConfig.json"),
            cancellationToken);
        
        var akkaConfig = ConfigurationFactory.ParseString(configuration);
        var actorSystemSetup = BootstrapSetup
            .Create()
            .WithConfig(akkaConfig)
            .And(DependencyResolverSetup.Create(_serviceProvider));

        _actorSystem = ActorSystem.Create("HtmActorSystem", actorSystemSetup);
        _actorSystem.ActorOf(DependencyResolver.For(_actorSystem).Props<HtmActor>(), nameof(HtmActor));

        await Task.Delay(TimeSpan.FromSeconds(1), cancellationToken);
        
        var requestHandlerActor = await _actorSystem
            .ActorSelection("akka.tcp://HtmActorSystem@localhost:5005/user/HtmActor/RequestHandlerActor")
            .ResolveOne(TimeSpan.FromSeconds(5), cancellationToken);

        var htmActorBridge = _serviceProvider.GetRequiredService<HtmActorBridge>();
        htmActorBridge.AddRequestHandlerActor(requestHandlerActor);
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _actorSystem?.Terminate();
        return Task.CompletedTask;
    }
}