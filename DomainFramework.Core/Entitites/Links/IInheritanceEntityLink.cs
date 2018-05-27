namespace DomainFramework.Core
{
    public interface IInheritanceEntityLink : IEntityLink
    {
        IEntity GetLinkedEntity();
    }

    public interface IInheritanceEntityLink<T> : IInheritanceEntityLink
       where T : IEntity
    {
        T LinkedEntity { get; set; }
    }
}