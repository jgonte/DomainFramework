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

        /// <summary>
        /// Retrieves the default value of the key to know where ti Insert or Update in case of Save
        /// </summary>
        /// <returns></returns>
        object GetDefaultIdentifierValue();
    }
}