namespace Gremlin.Linq.Linq
{
    public interface IWhereSelector : IGremlinQueryable
    {
    }

    public interface IWhereSelector<T> : IWhereSelector
    {
    }
}