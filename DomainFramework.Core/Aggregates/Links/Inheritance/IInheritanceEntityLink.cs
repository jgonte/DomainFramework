namespace DomainFramework.Core
{
    public interface IInheritanceEntityLink : IEntityLink
    {
        /// <summary>
        /// Retrieves the entity to persist or the one that was read
        /// </summary>
        /// <returns></returns>
        IEntity GetLinkedEntity();
    }

    public interface IInheritanceEntityLink<TLinkedEntity> : IInheritanceEntityLink
    {
        TLinkedEntity LinkedEntity { get; set; }
    }
}