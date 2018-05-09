namespace Gremlin.Linq.Linq
{
    public abstract class EdgeSelector : Selector
    {
        protected EdgeSelector(IGraphClient graphClient) : base(graphClient)
        {
        }
    }
}