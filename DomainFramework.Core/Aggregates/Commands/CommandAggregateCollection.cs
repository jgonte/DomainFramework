﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DomainFramework.Core
{
    public class CommandAggregateCollection<TAggregate, TEntity> : ICommandAggregateCollection<TAggregate, TEntity>
        where TAggregate : ICommandAggregate<TEntity>
    {
        public IRepositoryContext RepositoryContext { get; set; }

        public IEnumerable<TAggregate> Aggregates { get; set; }

        public bool RequiresUnitOfWork => Aggregates.Count() > 1;

        public CommandAggregateCollection(RepositoryContext context)
        {
            RepositoryContext = context;
        }

        public void Save(IAuthenticatedUser user = null, IUnitOfWork unitOfWork = null)
        {
            var ownsUnitOfWork = false;

            if (unitOfWork == null && RequiresUnitOfWork)
            {
                unitOfWork = RepositoryContext.CreateUnitOfWork();

                ownsUnitOfWork = true;
            }

            foreach (var aggregate in Aggregates)
            {
                aggregate.Save(user, unitOfWork);
            }

            if (ownsUnitOfWork)
            {
                unitOfWork.Save();
            }
        }

        public async Task SaveAsync(IAuthenticatedUser user = null, IUnitOfWork unitOfWork = null)
        {
            var ownsUnitOfWork = false;

            if (unitOfWork == null && RequiresUnitOfWork)
            {
                unitOfWork = RepositoryContext.CreateUnitOfWork();

                ownsUnitOfWork = true;
            }

            var tasks = new List<Task>();

            foreach (var aggregate in Aggregates)
            {
                var task = aggregate.SaveAsync(user, unitOfWork);

                tasks.Add(task);
            }

            await Task.WhenAll(tasks);

            if (ownsUnitOfWork)
            {
                await unitOfWork.SaveAsync();
            }
        }

        public void Delete(IAuthenticatedUser user = null, IUnitOfWork unitOfWork = null)
        {
            var ownsUnitOfWork = false;

            if (unitOfWork == null && RequiresUnitOfWork)
            {
                unitOfWork = RepositoryContext.CreateUnitOfWork();

                ownsUnitOfWork = true;
            }

            foreach (var aggregate in Aggregates)
            {
                aggregate.Delete(user, unitOfWork);
            }

            if (ownsUnitOfWork)
            {
                unitOfWork.Save();
            }
        }

        public async Task DeleteAsync(IAuthenticatedUser user = null, IUnitOfWork unitOfWork = null)
        {
            var ownsUnitOfWork = false;

            if (unitOfWork == null && RequiresUnitOfWork)
            {
                unitOfWork = RepositoryContext.CreateUnitOfWork();

                ownsUnitOfWork = true;
            }

            var tasks = new List<Task>();

            foreach (var aggregate in Aggregates)
            {
                var task = aggregate.DeleteAsync(user, unitOfWork);

                tasks.Add(task);
            }

            await Task.WhenAll(tasks);

            if (ownsUnitOfWork)
            {
                await unitOfWork.SaveAsync();
            }
        }
    }
}