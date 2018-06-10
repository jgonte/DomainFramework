namespace DomainFramework.Core
{
    public interface IInheritanceTransactedOperation : ITransactedOperation
    {
        /// <summary>
        /// The repository to share among the hierarchy of inheritance transacted operations
        /// </summary>
        ICommandRepository Repository { get; set; }
    }
}
