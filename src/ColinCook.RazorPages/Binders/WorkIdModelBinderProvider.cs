using ColinCook.VisitWorkflow.Identities;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Threading.Tasks;

namespace ColinCook.RazorPages.Binders
{
    public class WorkIdModelBinderProvider : IModelBinderProvider
    {
        public IModelBinder GetBinder(ModelBinderProviderContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (context.Metadata.ModelType == typeof(WorkId))
            {
                return new WorkIdModelBinder();
            }

            return null;
        }
    }

    public class WorkIdModelBinder : IModelBinder
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

            if (!WorkId.IsValid(identity))
            {
                bindingContext.ModelState.TryAddModelError(bindingContext.ModelName, "Not a valid WorkId");
                return Task.CompletedTask;
            }

            WorkId workId = new WorkId(identity);

            bindingContext.Result = ModelBindingResult.Success(workId);
            return Task.CompletedTask;
        }
    }
}