using System;

namespace DomainFramework.Core
{
    /// <summary>
    /// Defines an entity
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    public abstract class Entity<TKey> : IEntity, IEquatable<Entity<TKey>>
    {
        // Since we are using the convention of null id to insert and non-null id for update
        // do not make this property virtual
        public TKey Id { get; set; }

        public object GetDefaultIdentifierValue()
        {
            return default(TKey);
        }

        object IEntity.Id { get => Id; set => Id = (TKey)value; }

        ///// <summary>
        ///// Required since a template constructor cannot receive arguments
        ///// </summary>
        //public Entity()
        //{
        //}

        //protected Entity(TKey id)
        //{
        //    Id = id;
        //}

        public override bool Equals(object obj)
        {
            if (obj != null)
            {
                return Equals(obj as Entity<TKey>);
            }

            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        #region IEquatable Members

        public bool Equals(Entity<TKey> other)
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
