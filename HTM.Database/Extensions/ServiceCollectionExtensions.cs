using HTM.Infrastructure.Akka;
using HTM.Infrastructure.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HTM.Database.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddDatabase(this IServiceCollection services)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile(Application.AppSettingsFile)
            .Build();
        
        var connectionString = configuration.GetSection("Database")["ConnectionString"];
        
        services.AddDbContext<HtmContext>(options => 
            options.UseSqlServer(connectionString));
    }
}