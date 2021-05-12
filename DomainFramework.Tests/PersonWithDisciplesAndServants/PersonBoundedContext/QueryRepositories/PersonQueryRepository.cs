using DataAccess;
using DomainFramework.Core;
using DomainFramework.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Utilities;

namespace PersonWithDisciplesAndServants.PersonBoundedContext
{
    public class PersonQueryRepository : EntityQueryRepository<Person, int>
    {
        public override (int, IEnumerable<Person>) Get(CollectionQueryParameters queryParameters)
        {
            var result = Query<Person>
                .Collection()
                .Connection(PersonWithDisciplesAndServantsConnectionClass.GetConnectionName())
                .StoredProcedure("[PersonBoundedContext].[pPerson_Get]")
                .QueryParameters(queryParameters)
                .Parameters(p => p.Name("count").Count())
                .Execute();

            return (result.Count, result.Records);
        }

        public async override Task<(int, IEnumerable<Person>)> GetAsync(CollectionQueryParameters queryParameters)
        {
            var result = await Query<Person>
                .Collection()
                .Connection(PersonWithDisciplesAndServantsConnectionClass.GetConnectionName())
                .StoredProcedure("[PersonBoundedContext].[pPerson_Get]")
                .QueryParameters(queryParameters)
                .Parameters(p => p.Name("count").Count())
                .ExecuteAsync();

            return (result.Count, result.Records);
        }

        public override IEnumerable<Person> GetAll()
        {
            var result = Query<Person>
                .Collection()
                .Connection(PersonWithDisciplesAndServantsConnectionClass.GetConnectionName())
                .StoredProcedure("[PersonBoundedContext].[pPerson_GetAll]")
                .Execute();

            return result.Records;
        }

        public async override Task<IEnumerable<Person>> GetAllAsync()
        {
            var result = await Query<Person>
                .Collection()
                .Connection(PersonWithDisciplesAndServantsConnectionClass.GetConnectionName())
                .StoredProcedure("[PersonBoundedContext].[pPerson_GetAll]")
                .ExecuteAsync();

            return result.Records;
        }

        public override Person GetById(int personId)
        {
            var result = Query<Person>
                .Single()
                .Connection(PersonWithDisciplesAndServantsConnectionClass.GetConnectionName())
                .StoredProcedure("[PersonBoundedContext].[pPerson_GetById]")
                .Parameters(
                    p => p.Name("personId").Value(personId)
                )
                .Execute();

            return result.Record;
        }

        public async override Task<Person> GetByIdAsync(int personId)
        {
            var result = await Query<Person>
                .Single()
                .Connection(PersonWithDisciplesAndServantsConnectionClass.GetConnectionName())
                .StoredProcedure("[PersonBoundedContext].[pPerson_GetById]")
                .Parameters(
                    p => p.Name("personId").Value(personId)
                )
                .ExecuteAsync();

            return result.Record;
        }

        public IEnumerable<Person> GetAllDisciplesForPerson(int personId)
        {
            var result = Query<Person>
                .Collection()
                .Connection(PersonWithDisciplesAndServantsConnectionClass.GetConnectionName())
                .StoredProcedure("[PersonBoundedContext].[pPerson_GetAllDisciples]")
                .Parameters(
                    p => p.Name("personId").Value(personId)
                )
                .Execute();

            return result.Records;
        }

        public async Task<IEnumerable<Person>> GetAllDisciplesForPersonAsync(int personId)
        {
            var result = await Query<Person>
                .Collection()
                .Connection(PersonWithDisciplesAndServantsConnectionClass.GetConnectionName())
                .StoredProcedure("[PersonBoundedContext].[pPerson_GetAllDisciples]")
                .Parameters(
                    p => p.Name("personId").Value(personId)
                )
                .ExecuteAsync();

            return result.Records;
        }

        public IEnumerable<Person> GetAllServantsForPerson(int personId)
        {
            var result = Query<Person>
                .Collection()
                .Connection(PersonWithDisciplesAndServantsConnectionClass.GetConnectionName())
                .StoredProcedure("[PersonBoundedContext].[pPerson_GetAllServants]")
                .Parameters(
                    p => p.Name("personId").Value(personId)
                )
                .Execute();

            return result.Records;
        }

        public async Task<IEnumerable<Person>> GetAllServantsForPersonAsync(int personId)
        {
            var result = await Query<Person>
                .Collection()
                .Connection(PersonWithDisciplesAndServantsConnectionClass.GetConnectionName())
                .StoredProcedure("[PersonBoundedContext].[pPerson_GetAllServants]")
                .Parameters(
                    p => p.Name("personId").Value(personId)
                )
                .ExecuteAsync();

            return result.Records;
        }

        public (int, IEnumerable<Person>) GetDisciplesForPerson(int personId, CollectionQueryParameters queryParameters)
        {
            var result = Query<Person>
                .Collection()
                .Connection(PersonWithDisciplesAndServantsConnectionClass.GetConnectionName())
                .StoredProcedure("[PersonBoundedContext].[pPerson_GetDisciples]")
                .QueryParameters(queryParameters)
                .Parameters(p => p.Name("count").Count())
                .Parameters(
                    p => p.Name("personId").Value(personId)
                )
                .Execute();

            return (result.Count, result.Records);
        }

        public async Task<(int, IEnumerable<Person>)> GetDisciplesForPersonAsync(int personId, CollectionQueryParameters queryParameters)
        {
            var result = await Query<Person>
                .Collection()
                .Connection(PersonWithDisciplesAndServantsConnectionClass.GetConnectionName())
                .StoredProcedure("[PersonBoundedContext].[pPerson_GetDisciples]")
                .QueryParameters(queryParameters)
                .Parameters(p => p.Name("count").Count())
                .Parameters(
                    p => p.Name("personId").Value(personId)
                )
                .ExecuteAsync();

            return (result.Count, result.Records);
        }

        public (int, IEnumerable<Person>) GetServantsForPerson(int personId, CollectionQueryParameters queryParameters)
        {
            var result = Query<Person>
                .Collection()
                .Connection(PersonWithDisciplesAndServantsConnectionClass.GetConnectionName())
                .StoredProcedure("[PersonBoundedContext].[pPerson_GetServants]")
                .QueryParameters(queryParameters)
                .Parameters(p => p.Name("count").Count())
                .Parameters(
                    p => p.Name("personId").Value(personId)
                )
                .Execute();

            return (result.Count, result.Records);
        }

        public async Task<(int, IEnumerable<Person>)> GetServantsForPersonAsync(int personId, CollectionQueryParameters queryParameters)
        {
            var result = await Query<Person>
                .Collection()
                .Connection(PersonWithDisciplesAndServantsConnectionClass.GetConnectionName())
                .StoredProcedure("[PersonBoundedContext].[pPerson_GetServants]")
                .QueryParameters(queryParameters)
                .Parameters(p => p.Name("count").Count())
                .Parameters(
                    p => p.Name("personId").Value(personId)
                )
                .ExecuteAsync();

            return (result.Count, result.Records);
        }

        public static void Register(DomainFramework.DataAccess.RepositoryContext context)
        {
            context.RegisterQueryRepository<Person>(new PersonQueryRepository());
        }

    }
}