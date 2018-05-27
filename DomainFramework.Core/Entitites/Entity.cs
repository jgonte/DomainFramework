using System;

namespace DomainFramework.Core
{
<<<<<<< HEAD
    /// <summary>
    /// Defines an object as an entity by inheriting from it
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    public abstract class Entity<TKey> : IEquatable<Entity<TKey>>,
        IEntity<TKey>
    {
        // Since we are using the convention of null id to insert and non-null id for update
        // do not make this property virtual
        public TKey Id { get; set; }

        public object GetId() => Id;

=======
    public abstract class Entity<K, T> : IEquatable<Entity<K, T>>,
        IEntity<K, T>
    {
        public K Id { get; internal set; }

        public object GetId() => Id;

        public T Data { get; internal set; }

        public object GetData() => Data;
>>>>>>> bd9dc060af59b6a4d3f8b8d2e65aaf2f692497d3

        /// <summary>
        /// Required since a template constructor cannot receive arguments
        /// </summary>
        public Entity()
        {
        }

<<<<<<< HEAD
        protected Entity(TKey id)
        {
            Id = id;
=======
        protected Entity(T data, K id)
        {
            Id = id;

            Data = data;
>>>>>>> bd9dc060af59b6a4d3f8b8d2e65aaf2f692497d3
        }

        public override bool Equals(object obj)
        {
            if (obj != null)
            {
<<<<<<< HEAD
                return Equals(obj as Entity<TKey>);
=======
                return Equals(obj as Entity<K, T>);
>>>>>>> bd9dc060af59b6a4d3f8b8d2e65aaf2f692497d3
            }

            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        #region IEquatable Members

<<<<<<< HEAD
        public bool Equals(Entity<TKey> other)
=======
        public bool Equals(Entity<K, T> other)
>>>>>>> bd9dc060af59b6a4d3f8b8d2e65aaf2f692497d3
        {
            if (other == null)
            {
                return false;
            }

            return Id.Equals(other.Id);
        }

        #endregion
    }
}
