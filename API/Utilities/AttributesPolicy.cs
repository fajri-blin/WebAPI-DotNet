namespace API.Utilities;

public class AttributesPolicy<TIEntityRepository,TEntity>
{
    private readonly TEntity _entity;

    public AttributesPolicy(TEntity entity)
    {
        _entity = entity;
    }
}

