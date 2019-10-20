using DomainFramework.Core;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace DomainFramework.DataAccess
{
    public class CollectionQueryParametersModelBinderProvider : IModelBinderProvider
    {
        public IModelBinder GetBinder(ModelBinderProviderContext context) => context.Metadata.ModelType == typeof(CollectionQueryParameters) ?
                new CollectionQueryParametersModelBinder() :
                null;
    }
}
