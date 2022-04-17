using HTM.Core.Adapters;
using HTM.Database.Entities;
using HTM.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace HTM.Database.Dao;

public class TemperatureMeasurementDao : ITemperatureMeasurementDao
{
    private readonly HtmContext _context;

    public TemperatureMeasurementDao(HtmContext context)
    {
        _context = context;
    }

    public async Task AddMeasurement(TemperatureMeasurement temperatureMeasurement)
    {
        await _context.TemperatureMeasurements.AddAsync(new TemperatureMeasurementDb(temperatureMeasurement));
        await _context.SaveChangesAsync();
    }

    public async Task<TemperatureMeasurement> GetLastMeasurement()
    {
        var measurement = await _context.TemperatureMeasurements
            .OrderByDescending(x => x.MeasurementDate)
            .FirstOrDefaultAsync();

        return measurement?.ToModel();
    }

    public async Task<TemperatureMeasurement[]> GetMeasurementsByDateRange(DateTime from, DateTime to)
    {
        return await _context.TemperatureMeasurements
            .OrderBy(x => x.MeasurementDate)
            .Where(x => x.MeasurementDate >= from && x.MeasurementDate <= to)
            .Select(x => x.ToModel())
            .ToArrayAsync();
    }
}