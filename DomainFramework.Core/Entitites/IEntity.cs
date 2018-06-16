namespace DomainFramework.Core
{
    /// <summary>
    /// Defines an entity
    /// </summary>
    public interface IEntity
    {
        /// <summary>
        /// The identifier of the entity
        /// </summary>
        object Id { get; set; }
    }
}