﻿using DataAccess;
using DomainFramework.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DomainFramework.Tests.EmployeeWithSpouse
{
    public class PersonQueryRepository : EntityQueryRepository<Person, int?>
    {
        public override Person GetById(int? personId, IAuthenticatedUser user)
        {
            var result = Query<Person>
                .Single()
                .Connection(TestConnectionClass.GetConnectionName())
                .StoredProcedure("[pPerson_GetById]")
                .Parameters(
                    p => p.Name("personId").Value(personId.Value)
                )
                .MapTypes(
                    8,
                    tm => tm.Type(typeof(Employee)).Index(1),
                    tm => tm.Type(typeof(Person)).Index(2)
                )
                .Execute();

            return result.Data;
        }

        public async override Task<Person> GetByIdAsync(int? personId, IAuthenticatedUser user)
        {
            var result = await Query<Person>
                .Single()
                .Connection(TestConnectionClass.GetConnectionName())
                .StoredProcedure("[pPerson_GetById]")
                .Parameters(
                    p => p.Name("personId").Value(personId.Value)
                )
                .MapTypes(
                    8,
                    tm => tm.Type(typeof(Employee)).Index(1),
                    tm => tm.Type(typeof(Person)).Index(2)
                )
                .ExecuteAsync();

            return result.Data;
        }

        public override IEnumerable<Person> Get(QueryParameters queryParameters, IAuthenticatedUser user)
        {
            var result = Query<Person>
                .Collection()
                .Connection(TestConnectionClass.GetConnectionName())
                .StoredProcedure("[pPerson_Get]")
                .MapTypes(
                    8,
                    tm => tm.Type(typeof(Employee)).Index(1),
                    tm => tm.Type(typeof(Person)).Index(2)
                )
                .Execute();

            return result.Data;
        }

        public async override Task<IEnumerable<Person>> GetAsync(QueryParameters queryParameters, IAuthenticatedUser user)
        {
            var result = await Query<Person>
                .Collection()
                .Connection(TestConnectionClass.GetConnectionName())
                .StoredProcedure("[pPerson_Get]")
                .MapTypes(
                    8,
                    tm => tm.Type(typeof(Employee)).Index(1),
                    tm => tm.Type(typeof(Person)).Index(2)
                )
                .ExecuteAsync();

            return result.Data;
        }

        public Person GetSpouseForPerson(int? marriedToPersonId, IAuthenticatedUser user)
        {
            var result = Query<Person>
                .Single()
                .Connection(TestConnectionClass.GetConnectionName())
                .StoredProcedure("[pPerson_GetSpouse_ForPerson]")
                .Parameters(
                    p => p.Name("marriedToPersonId").Value(marriedToPersonId.Value)
                )
                .MapTypes(
                    7,
                    tm => tm.Type(typeof(Employee)).Index(1),
                    tm => tm.Type(typeof(Person)).Index(2)
                )
                .Execute();

            return result.Data;
        }

        public async Task<Person> GetSpouseForPersonAsync(int? marriedToPersonId, IAuthenticatedUser user)
        {
            var result = await Query<Person>
                .Single()
                .Connection(TestConnectionClass.GetConnectionName())
                .StoredProcedure("[pPerson_GetSpouse_ForPerson]")
                .Parameters(
                    p => p.Name("marriedToPersonId").Value(marriedToPersonId.Value)
                )
                .MapTypes(
                    7,
                    tm => tm.Type(typeof(Employee)).Index(1),
                    tm => tm.Type(typeof(Person)).Index(2)
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
