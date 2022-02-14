using Grpc.Core;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace HTM.Web.Communication.Services;

public class GrpcHostedService : IHostedService
{
    private readonly Server _server;

    public GrpcHostedService(Server server)
    {
        _server = server;
    }
    
    public Task StartAsync(CancellationToken cancellationToken)
    {
        _server.Start();
        
        Log.Information("{GrpcHostedService} | Server Started", nameof(GrpcHostedService));
        
        return Task.CompletedTask;
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        await _server.ShutdownAsync();
        
        Log.Information("{GrpcHostedService} | Server Stopped", nameof(GrpcHostedService));
    }
}