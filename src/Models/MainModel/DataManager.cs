using MainModel.DataProviders.EntityFramework;
using MainModel.DataProviders.EntityFramework.Repositories;
using MainModel.Repositories;

public record DataManager 
    (ICoordinate Coordinate, IPoint Point, IPollution Pollution, IPollutionSet PollutionSet)
{
    public static DataManager Set(EfProvider provider)
    {
        switch (provider)
        {
            case EfProvider.SqlServer:
                throw new NotSupportedException("В работе");
            case EfProvider.SqLite:
                EfDbContext context = new EfDbContext(provider);
                return new DataManager(new CoordinateRep(context), new PointRep(context), 
                    new PollutionRep(context), new PollutionSetRep(context));
            default:
                throw new NotSupportedException("Что-то пошло не так!");

        }
    }
}