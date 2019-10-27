using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace DomainFramework.DataAccess
{
    public class CollectionQueryParametersModelBinder : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            if (bindingContext == null)
            {
                throw new ArgumentNullException(nameof(bindingContext));
            }

            var model = new CollectionQueryParametersBuilder(bindingContext.ActionContext.HttpContext.Request.Query)
                .Build();

            bindingContext.Result = ModelBindingResult.Success(model);

            return Task.CompletedTask;
        }
    }
}
