﻿using DataAccess;
using DomainFramework.Core;
using System.Linq;
using Utilities;

namespace DomainFramework.Tests
{
    class CapitalCityCommandRepository : DataAccess.EntityCommandRepository<CapitalCityEntity>
    {
        protected override Command CreateInsertCommand(CapitalCityEntity entity, IAuthenticatedUser user, string selector)
        {
            return Query<CapitalCityEntity>
                .Single()
                .Connection(ConnectionName)
                .StoredProcedure("p_CapitalCity_Create")
                .Parameters(
                    p => p.Name("name").Value(entity.Name)
                )
                .RecordInstance(entity)
                .MapProperties(
                    pm => pm.Map<CapitalCityEntity>(m => m.Id)//.Index(0),
                )
                .OnBeforeCommandExecuted(cmd =>
                {
                    var e = (CountryEntity)Dependencies().Single().Entity;

                    entity.CountryCode = e.Id;

                    cmd.Parameters( // Map the extra parameters for the foreign key(s)
                        p => p.Name("countryCode").Value(entity.CountryCode)
                    );
                });
        }

        protected override Command CreateUpdateCommand(CapitalCityEntity entity, IAuthenticatedUser user, string selector)
        {
            return Command
                .NonQuery()
                .Connection(ConnectionName)
                .StoredProcedure("p_CapitalCity_Update")
                .Parameters(
                    p => p.Name("capitalCityId").Value(entity.Id.Value),
                    p => p.Name("countryCode").Value(entity.CountryCode)
                )
                .RecordInstance(entity)
                .AutoGenerateParameters();
        }

        protected override void HandleInsert(Command command)
        {
            ((SingleQuery<CapitalCityEntity>)command).Execute();
        }
    }
}