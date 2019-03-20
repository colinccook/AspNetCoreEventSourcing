using ColinCook.VisitWorkflow.Identities;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Threading.Tasks;

namespace ColinCook.RazorPages.Binders
{
    public class OperativeIdModelBinderProvider : IModelBinderProvider
    {
        public IModelBinder GetBinder(ModelBinderProviderContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (context.Metadata.ModelType == typeof(OperativeId))
            {
                return new OperativeIdModelBinder();
            }

            return null;
        }
    }

    public class OperativeIdModelBinder : IModelBinder
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

            if (!OperativeId.IsValid(identity))
            {
                bindingContext.ModelState.TryAddModelError(bindingContext.ModelName, "Not a valid OperativeId");
                return Task.CompletedTask;
            }

            OperativeId operativeId = new OperativeId(identity);

            bindingContext.Result = ModelBindingResult.Success(operativeId);
            return Task.CompletedTask;
        }
    }
}