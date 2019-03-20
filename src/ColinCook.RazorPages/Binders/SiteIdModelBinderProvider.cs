using ColinCook.VisitWorkflow.Identities;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Threading.Tasks;

namespace ColinCook.RazorPages.Binders
{
    public class SiteIdModelBinderProvider : IModelBinderProvider
    {
        public IModelBinder GetBinder(ModelBinderProviderContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (context.Metadata.ModelType == typeof(SiteId))
            {
                return new SiteIdModelBinder();
            }

            return null;
        }
    }

    public class SiteIdModelBinder : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            if (bindingContext == null)
            {
                throw new ArgumentNullException(nameof(bindingContext));
            }

            string modelName = bindingContext.ModelName;
            ValueProviderResult valueProviderResult = bindingContext.ValueProvider.GetValue(modelName);

            if (valueProviderResult == ValueProviderResult.None)
            {
                return Task.CompletedTask;
            }

            bindingContext.ModelState.SetModelValue(modelName, valueProviderResult);

            string identity = valueProviderResult.FirstValue;

            if (!SiteId.IsValid(identity))
            {
                bindingContext.ModelState.TryAddModelError(bindingContext.ModelName, "Not a valid SiteId");
                return Task.CompletedTask;
            }

            SiteId siteId = new SiteId(identity);

            bindingContext.Result = ModelBindingResult.Success(siteId);
            return Task.CompletedTask;
        }
    }
}