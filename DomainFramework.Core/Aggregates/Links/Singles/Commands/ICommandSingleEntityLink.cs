﻿using System.Threading.Tasks;

namespace DomainFramework.Core
{
    /// <summary>
    /// A more generic interface to be used in the aggregate to enforce the same type as the root entity of the aggregate
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public interface ICommandSingleEntityLink<TEntity> : ISingleEntityLink
    {
        void Save(IRepositoryContext repositoryContext, IUnitOfWork unitOfWork, TEntity rootEntity);

        Task SaveAsync(IRepositoryContext repositoryContext, IUnitOfWork unitOfWork, TEntity rootEntity);
    }

    /// <summary>
    /// More specific interface to enforce the linked type
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TLinkedEntity"></typeparam>
    public interface ICommandSingleEntityLink<TEntity, TLinkedEntity> : ICommandSingleEntityLink<TEntity>,
        ISingleEntityLink<TLinkedEntity>
    {
        void SetEntity(TLinkedEntity entity);
    }
}