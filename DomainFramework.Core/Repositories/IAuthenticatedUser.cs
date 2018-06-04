namespace DomainFramework.Core
{
    /// <summary>
    /// Information about the logged in user
    /// </summary>
    public interface IAuthenticatedUser
    {
        /// <summary>
        /// The id of the user
        /// </summary>
        object Id { get; set; }
    }
}
