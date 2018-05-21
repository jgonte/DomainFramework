using System;

namespace DomainFramework.Core
{
    public abstract class Entity<K, T> : IEquatable<Entity<K, T>>,
        IEntity<K, T>
    {
        public K Id { get; internal set; }

        public object GetId() => Id;

        public T Data { get; internal set; }

        public object GetData() => Data;

        /// <summary>
        /// Required since a template constructor cannot receive arguments
        /// </summary>
        public Entity()
        {
        }

        protected Entity(T data, K id)
        {
            Id = id;

            Data = data;
        }

        public override bool Equals(object obj)
        {
            if (obj != null)
            {
                return Equals(obj as Entity<K, T>);
            }

            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        #region IEquatable Members

        public bool Equals(Entity<K, T> other)
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
