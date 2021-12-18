namespace MainModel.Entities;
public abstract class EntityBase : ValueObject
    {
    public Guid Id { get; init; }
    protected override IEnumerable<object> GetEqualityComponents() 
    {
        yield return Id;
    }

}
