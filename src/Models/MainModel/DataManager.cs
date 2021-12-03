using MainModel.Repositories;

public record DataManager 
    (ICoordinate Coordinate, IPoint Point, IPollution Pollution, IPollutionSet PollutionSet);