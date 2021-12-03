using MainModel.Entities;
using Microsoft.EntityFrameworkCore;

namespace MainModel.DataProviders.EntityFramework;

public class EfDbContext : DbContext
{
    public DbSet<Coordinate> Coordinates { get; set; } = null!;
    public DbSet<Point> Points { get; set; } = null!;
    public DbSet<Pollution> Pollutions { get; set; } = null!;
    public DbSet<PollutionSet> PollutionSets { get; set; } = null!;

    private readonly EfProvider provider;
    public EfDbContext(EfProvider provider)
    {
        this.provider = provider;
        Database.EnsureCreated();
    }

    protected override void OnConfiguring(DbContextOptionsBuilder builder)
    {
        switch (provider)
        {
            case EfProvider.SqlServer:
                throw new NotSupportedException("В работе");
            case EfProvider.SqLite:
                builder.UseSqlite(
                    @"Data Source = C:\Users\Root\source\repos\TheEmisssionsMapOfOmsk\src\Data\Map.db");
                return;
            default:
                throw new NotSupportedException("Что-то пошло не так!");

        }

    }
}

