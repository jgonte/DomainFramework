namespace DomainFramework.Core
{
    /// <summary>
    /// Entity that wraps a data object to provide relational persistence for it
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TEntity"></typeparam>
    public abstract class ContainerEntity<TKey, TEntity> : Entity<TKey>
    {
        public TEntity Data { get; set; }

        public object GetData() => Data;

        /// <summary>
        /// Required since a template constructor cannot receive arguments
        /// </summary>
        public ContainerEntity()
        {
        }

        protected ContainerEntity(TEntity data, TKey id)
        {
            Data = data;

            Id = id;
        }
    }
}
