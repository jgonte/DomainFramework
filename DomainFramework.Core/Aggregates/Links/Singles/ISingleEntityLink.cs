namespace DomainFramework.Core
{
    public interface ISingleEntityLink : IEntityLink
    {
        IEntity GetLinkedEntity();
    }

    public interface ISingleEntityLink<TLinkedEntity> : ISingleEntityLink
        where TLinkedEntity : IEntity
    {
        TLinkedEntity LinkedEntity { get; set; }
    }
}