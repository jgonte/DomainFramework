using DataAccess;
using DomainFramework.Core;
using DomainFramework.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PersonWithDisciples.PersonBoundedContext
{
    public class PersonQueryRepository : EntityQueryRepository<Person, int>
    {
        public override (int, IEnumerable<Person>) Get(CollectionQueryParameters queryParameters)
        {
            var result = Query<Person>
                .Collection()
                .Connection(PersonWithDisciplesConnectionClass.GetConnectionName())
                .StoredProcedure("[PersonBoundedContext].[pPerson_Get]")
                .QueryParameters(queryParameters)
                .Parameters(p => p.Name("count").Size(20).Output())
                .Execute();

            var count = (string)result.GetParameter("count").Value;

            return (int.Parse(count), result.Data);
        }

        public async override Task<(int, IEnumerable<Person>)> GetAsync(CollectionQueryParameters queryParameters)
        {
            var result = await Query<Person>
                .Collection()
                .Connection(PersonWithDisciplesConnectionClass.GetConnectionName())
                .StoredProcedure("[PersonBoundedContext].[pPerson_Get]")
                .QueryParameters(queryParameters)
                .Parameters(p => p.Name("count").Size(20).Output())
                .ExecuteAsync();

            var count = (string)result.GetParameter("count").Value;

            return (int.Parse(count), result.Data);
        }

        public override IEnumerable<Person> GetAll()
        {
            var result = Query<Person>
                .Collection()
                .Connection(PersonWithDisciplesConnectionClass.GetConnectionName())
                .StoredProcedure("[PersonBoundedContext].[pPerson_GetAll]")
                .Execute();

            return result.Data;
        }

        public async override Task<IEnumerable<Person>> GetAllAsync()
        {
            var result = await Query<Person>
                .Collection()
                .Connection(PersonWithDisciplesConnectionClass.GetConnectionName())
                .StoredProcedure("[PersonBoundedContext].[pPerson_GetAll]")
                .ExecuteAsync();

            return result.Data;
        }

        public override Person GetById(int personId)
        {
            var result = Query<Person>
                .Single()
                .Connection(PersonWithDisciplesConnectionClass.GetConnectionName())
                .StoredProcedure("[PersonBoundedContext].[pPerson_GetById]")
                .Parameters(
                    p => p.Name("personId").Value(personId)
                )
                .Execute();

            return result.Data;
        }

        public async override Task<Person> GetByIdAsync(int personId)
        {
            var result = await Query<Person>
                .Single()
                .Connection(PersonWithDisciplesConnectionClass.GetConnectionName())
                .StoredProcedure("[PersonBoundedContext].[pPerson_GetById]")
                .Parameters(
                    p => p.Name("personId").Value(personId)
                )
                .ExecuteAsync();

            return result.Data;
        }

        public IEnumerable<Person> GetAllDisciplesForPerson(int personId)
        {
            var result = Query<Person>
                .Collection()
                .Connection(PersonWithDisciplesConnectionClass.GetConnectionName())
                .StoredProcedure("[PersonBoundedContext].[pPerson_GetAllDisciples]")
                .Parameters(
                    p => p.Name("personId").Value(personId)
                )
                .Execute();

            return result.Data;
        }

        public async Task<IEnumerable<Person>> GetAllDisciplesForPersonAsync(int personId)
        {
            var result = await Query<Person>
                .Collection()
                .Connection(PersonWithDisciplesConnectionClass.GetConnectionName())
                .StoredProcedure("[PersonBoundedContext].[pPerson_GetAllDisciples]")
                .Parameters(
                    p => p.Name("personId").Value(personId)
                )
                .ExecuteAsync();

            return result.Data;
        }

        public (int, IEnumerable<Person>) GetDisciplesForPerson(int personId, CollectionQueryParameters queryParameters)
        {
            var result = Query<Person>
                .Collection()
                .Connection(PersonWithDisciplesConnectionClass.GetConnectionName())
                .StoredProcedure("[PersonBoundedContext].[pPerson_GetDisciples]")
                .QueryParameters(queryParameters)
                .Parameters(p => p.Name("count").Size(20).Output())
                .Parameters(
                    p => p.Name("personId").Value(personId)
                )
                .Execute();

            var count = (string)result.GetParameter("count").Value;

            return (int.Parse(count), result.Data);
        }

        public async Task<(int, IEnumerable<Person>)> GetDisciplesForPersonAsync(int personId, CollectionQueryParameters queryParameters)
        {
            var result = await Query<Person>
                .Collection()
                .Connection(PersonWithDisciplesConnectionClass.GetConnectionName())
                .StoredProcedure("[PersonBoundedContext].[pPerson_GetDisciples]")
                .QueryParameters(queryParameters)
                .Parameters(p => p.Name("count").Size(20).Output())
                .Parameters(
                    p => p.Name("personId").Value(personId)
                )
                .ExecuteAsync();

            var count = (string)result.GetParameter("count").Value;

            return (int.Parse(count), result.Data);
        }

        public static void Register(DomainFramework.DataAccess.RepositoryContext context)
        {
            context.RegisterQueryRepository<Person>(new PersonQueryRepository());
        }

    }
}