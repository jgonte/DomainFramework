namespace DomainFramework.Tests.Entitites
{
    public interface IPersonRole
    {
        /// <summary>
        /// The person this role is assigned to
        /// </summary>
        int? PersonId { get; set; }
    }
}