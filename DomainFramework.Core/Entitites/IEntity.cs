namespace DomainFramework.Core
{
    public interface IEntity
    {
<<<<<<< HEAD
    }

    public interface IEntity<TKey> : IEntity
    {
        TKey Id { get; set; }
=======
        object GetId();

        object GetData();
    }

    public interface IEntity<K, T> : IEntity
    {
        K Id { get; }

        T Data { get; }
>>>>>>> bd9dc060af59b6a4d3f8b8d2e65aaf2f692497d3
    }
}