namespace DomainFramework.Core
{
    public interface ISingleEntityLink : IEntityLink
    {
        /// <summary>
        /// Retrieves the entity to persist or the one that was read
        /// </summary>
        /// <returns></returns>
        IEntity GetLinkedEntity();
    }

    public interface ISingleEntityLink<TLinkedEntity> : ISingleEntityLink
    {
        TLinkedEntity LinkedEntity { get; set; }
    }
}
