namespace DomainFramework.Core
{
    public class AuthenticatedUser<T> : IAuthenticatedUser
    {
        public T Id { get; private set; }

        object IAuthenticatedUser.Id { get => Id; set => Id = (T)value; }

        public AuthenticatedUser(T id)
        {
            Id = id;
        }
    }
}
