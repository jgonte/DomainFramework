namespace DomainFramework.Core
{
    public interface IAggregate
    {
        IRepositoryContext RepositoryContext { get; set; }

        IEntity RootEntity { get; set; }
    }
}