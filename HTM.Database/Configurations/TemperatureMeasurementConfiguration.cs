using HTM.Database.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HTM.Database.Configurations;

public class TemperatureMeasurementConfiguration: IEntityTypeConfiguration<TemperatureMeasurementDb>
{
    public void Configure(EntityTypeBuilder<TemperatureMeasurementDb> builder)
    {
        //builder.Property(x => x.Temperature).HasPrecision(18, 2);
    }
}