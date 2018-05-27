namespace DomainFramework.Core
{
    public interface ISingleEntityLink : IEntityLink
    {
        IEntity GetLinkedEntity();
    }

    public interface ISingleEntityLink<T> : ISingleEntityLink
        where T : IEntity
    {
        T LinkedEntity { get; set; }
    }
}