namespace DomainFramework.Core
{
    public interface IInheritanceEntityLink : IEntityLink
    {
        IEntity GetLinkedEntity();
    }

    public interface IInheritanceEntityLink<TLinkedEntity> : IInheritanceEntityLink
        where TLinkedEntity : IEntity
    {
        TLinkedEntity LinkedEntity { get; set; }
    }
}