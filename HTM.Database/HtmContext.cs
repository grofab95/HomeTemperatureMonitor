using HTM.Database.Entities;
using HTM.Infrastructure.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace HTM.Database;

public class HtmContext : DbContext
{
    public DbSet<TemperatureMeasurementDb> TemperatureMeasurements { get; set; }
    
    public HtmContext()
    {
                
    }
        
    public HtmContext(DbContextOptions<HtmContext> options) : base(options)
    {
        
    }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
       if (optionsBuilder.IsConfigured) 
           return;
        
       var appDirectory =
           Path.Combine(new DirectoryInfo(Directory.GetCurrentDirectory())?.Parent?.FullName ?? "HTM", "HTM");
       
        var configuration = new ConfigurationBuilder()
            .SetBasePath(appDirectory)
            .AddJsonFile(Application.AppSettingsFile)
            .Build();
        
        var connectionString = configuration.GetSection("Database")["ConnectionString"];
        optionsBuilder.UseSqlServer(connectionString);
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(HtmContext).Assembly); 
    }
}