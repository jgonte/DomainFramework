using DataAccess;
using DomainFramework.Core;
using DomainFramework.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PersonWithSpouseAndBestFriend.PersonBoundedContext
{
    public class PersonQueryRepository : EntityQueryRepository<Person, int>
    {
        public override (int, IEnumerable<Person>) Get(CollectionQueryParameters queryParameters)
        {
            var result = Query<Person>
                .Collection()
                .Connection(PersonWithSpouseAndBestFriendConnectionClass.GetConnectionName())
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
                .Connection(PersonWithSpouseAndBestFriendConnectionClass.GetConnectionName())
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
                .Connection(PersonWithSpouseAndBestFriendConnectionClass.GetConnectionName())
                .StoredProcedure("[PersonBoundedContext].[pPerson_GetAll]")
                .Execute();

            return result.Data;
        }

        public async override Task<IEnumerable<Person>> GetAllAsync()
        {
            var result = await Query<Person>
                .Collection()
                .Connection(PersonWithSpouseAndBestFriendConnectionClass.GetConnectionName())
                .StoredProcedure("[PersonBoundedContext].[pPerson_GetAll]")
                .ExecuteAsync();

            return result.Data;
        }

        public override Person GetById(int personId)
        {
            var result = Query<Person>
                .Single()
                .Connection(PersonWithSpouseAndBestFriendConnectionClass.GetConnectionName())
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
                .Connection(PersonWithSpouseAndBestFriendConnectionClass.GetConnectionName())
                .StoredProcedure("[PersonBoundedContext].[pPerson_GetById]")
                .Parameters(
                    p => p.Name("personId").Value(personId)
                )
                .ExecuteAsync();

            return result.Data;
        }

        public Person GetBestFriendOfForPerson(int personId)
        {
            var result = Query<Person>
                .Single()
                .Connection(PersonWithSpouseAndBestFriendConnectionClass.GetConnectionName())
                .StoredProcedure("[PersonBoundedContext].[pPerson_GetBestFriendOf]")
                .Parameters(
                    p => p.Name("personId").Value(personId)
                )
                .Execute();

            return result.Data;
        }

        public async Task<Person> GetBestFriendOfForPersonAsync(int personId)
        {
            var result = await Query<Person>
                .Single()
                .Connection(PersonWithSpouseAndBestFriendConnectionClass.GetConnectionName())
                .StoredProcedure("[PersonBoundedContext].[pPerson_GetBestFriendOf]")
                .Parameters(
                    p => p.Name("personId").Value(personId)
                )
                .ExecuteAsync();

            return result.Data;
        }

        public Person GetMarriedToForPerson(int personId)
        {
            var result = Query<Person>
                .Single()
                .Connection(PersonWithSpouseAndBestFriendConnectionClass.GetConnectionName())
                .StoredProcedure("[PersonBoundedContext].[pPerson_GetMarriedTo]")
                .Parameters(
                    p => p.Name("personId").Value(personId)
                )
                .Execute();

            return result.Data;
        }

        public async Task<Person> GetMarriedToForPersonAsync(int personId)
        {
            var result = await Query<Person>
                .Single()
                .Connection(PersonWithSpouseAndBestFriendConnectionClass.GetConnectionName())
                .StoredProcedure("[PersonBoundedContext].[pPerson_GetMarriedTo]")
                .Parameters(
                    p => p.Name("personId").Value(personId)
                )
                .ExecuteAsync();

            return result.Data;
        }

        public static void Register(DomainFramework.DataAccess.RepositoryContext context)
        {
            context.RegisterQueryRepository<Person>(new PersonQueryRepository());
        }

    }
}