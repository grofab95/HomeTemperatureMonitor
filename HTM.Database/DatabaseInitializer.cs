using HTM.Infrastructure.Adapters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace HTM.Database;

public class DatabaseInitializer : IPersistenceInitializer
{
    private readonly IServiceProvider _serviceProvider;

    public DatabaseInitializer(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }
    
    public async Task Initialize()
    {
        Log.Information("Migrating database");

        using var scope = _serviceProvider.CreateScope();
        await using var dbContext = scope.ServiceProvider.GetRequiredService<HtmContext>();
        await dbContext.Database.MigrateAsync();
        
        Log.Information("Migration complete");
    }
}