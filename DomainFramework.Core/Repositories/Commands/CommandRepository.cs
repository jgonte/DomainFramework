namespace DomainFramework.Core
{
    /// <summary>
    /// Repository that performs write operation on a database
    /// </summary>
    /// <typeparam name="E">The type of the entity</typeparam>
    /// <typeparam name="K">The type of the key of the entity</typeparam>
    /// <typeparam name="T">The type of the data of the entity</typeparam>
    public abstract class CommandRepository<E, K, T> : ICommandRepository
        where E : Entity<K, T>
    {
        public string ConnectionName { get; set; }

        public virtual void Save(E entity, IUnitOfWork unitOfWork = null)
        {
            if (entity.Id == null)
            {
                Insert(entity, unitOfWork);
            }
            else
            {
                Update(entity, unitOfWork);
            }
        }

        public abstract void Insert(E entity, IUnitOfWork unitOfWork = null);

        public abstract bool Update(E entity, IUnitOfWork unitOfWork = null);

        public abstract bool Delete(E entity, IUnitOfWork unitOfWork = null);

        protected void UpdateEntityId(E entity, K id)
        {
            entity.Id = id;
        }

        #region ICommandRepository members

        void ICommandRepository.Save(IEntity entity, IUnitOfWork unitOfWork)
        {
            Save((E)entity, unitOfWork);
        }

        void ICommandRepository.Insert(IEntity entity, IUnitOfWork unitOfWork)
        {
            Insert((E)entity);
        }

        bool ICommandRepository.Update(IEntity entity, IUnitOfWork unitOfWork)
        {
            return Update((E)entity);
        }

        bool ICommandRepository.Delete(IEntity entity, IUnitOfWork unitOfWork)
        {
            return Delete((E)entity);
        }

        #endregion
    }
}
