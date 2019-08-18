namespace DomainFramework.Core
{
    public interface IValueObject
    {
        /// <summary>
        /// Tells whether the value object does not contain any value(s) so the framework can return a null object if needed
        /// </summary>
        // Do not convert it to a property otherwise might generate a extra parameter to persist in the database
        bool IsEmpty();
    }
}