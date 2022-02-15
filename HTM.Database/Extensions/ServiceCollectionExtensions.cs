using HTM.Core.Adapters;
using HTM.Database.Dao;
using HTM.Infrastructure.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HTM.Database.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddDatabase(this IServiceCollection services)
    {
        var connectionString = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile(Application.AppSettingsFile)
            .Build()
            .GetSection("Database")["ConnectionString"];
        
        services.AddDbContext<HtmContext>(options => options.UseSqlServer(connectionString));
        services.AddScoped<ITemperatureMeasurementDao, TemperatureMeasurementDao>();
    }
}