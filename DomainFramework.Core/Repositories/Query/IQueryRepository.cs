namespace DomainFramework.Core
{
    /// <summary>
    /// Defines a read only repository
    /// </summary>
    public interface IQueryRepository : IRepository
    {
        IEntity GetById(object id);
    }
}