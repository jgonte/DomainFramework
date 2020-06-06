using DomainFramework.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleEntityWithAutoGeneratedKey.SimpleEntityWithAutoGeneratedKeyBoundedContext
{
    public class TestEntityQueryAggregate : GetByIdQueryAggregate<TestEntity, int?, TestEntityOutputDto>
    {
        public TestEntityQueryAggregate() : this(null)
        {
        }

        public TestEntityQueryAggregate(HashSet<(string, IEntity)> processedEntities = null) : base(new DomainFramework.DataAccess.RepositoryContext(SimpleEntityWithAutoGeneratedKeyConnectionClass.GetConnectionName()), processedEntities)
        {
            var context = (DomainFramework.DataAccess.RepositoryContext)RepositoryContext;

            TestEntityQueryRepository.Register(context);
        }

        public override void PopulateDto()
        {
            OutputDto.Id = RootEntity.Id.Value;

            OutputDto.Text = RootEntity.Text;

            OutputDto.IsActive = RootEntity.IsActive;

            OutputDto.IsEnumeration1Element1 = RootEntity.Enumeration1 == TestEntity.EnumerationType1.Element1;

            OutputDto.IsEnumeration1Element2 = RootEntity.Enumeration1 == TestEntity.EnumerationType1.Element2;

            OutputDto.IsEnumeration1Element3 = RootEntity.Enumeration1 == TestEntity.EnumerationType1.Element3;

            OutputDto.TypeValue1 = GetTypeValue1Dto();

            OutputDto.Url = GetUrlDto();

            OutputDto.Distance = GetDistanceDto();

            OutputDto.Traffic = GetTrafficDto();

            OutputDto.Time = GetTimeDto();
        }

        public TypeValueOutputDto GetTypeValue1Dto() => 
            new TypeValueOutputDto
            {
                DataType = RootEntity.TypeValue1.DataType,
                Data = RootEntity.TypeValue1.Data
            };

        public UrlOutputDto GetUrlDto() => 
            new UrlOutputDto
            {
                Value = RootEntity.Url.Value
            };

        public SelectionCriteriaOutputDto GetDistanceDto() => 
            (RootEntity.Distance.IsEmpty()) ? null : new SelectionCriteriaOutputDto
            {
                Selected = RootEntity.Distance.Selected,
                Yes = RootEntity.Distance.YesNoNotSure == "Y",
                No = RootEntity.Distance.YesNoNotSure == "N",
                NotSure = RootEntity.Distance.YesNoNotSure == "?"
            };

        public SelectionCriteriaOutputDto GetTrafficDto() => 
            (RootEntity.Traffic.IsEmpty()) ? null : new SelectionCriteriaOutputDto
            {
                Selected = RootEntity.Traffic.Selected,
                Yes = RootEntity.Traffic.YesNoNotSure == "Y",
                No = RootEntity.Traffic.YesNoNotSure == "N",
                NotSure = RootEntity.Traffic.YesNoNotSure == "?"
            };

        public SelectionCriteriaOutputDto GetTimeDto() => 
            (RootEntity.Time.IsEmpty()) ? null : new SelectionCriteriaOutputDto
            {
                Selected = RootEntity.Time.Selected,
                Yes = RootEntity.Time.YesNoNotSure == "Y",
                No = RootEntity.Time.YesNoNotSure == "N",
                NotSure = RootEntity.Time.YesNoNotSure == "?"
            };

    }
}