using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using ColinCook.VisitWorkflow.Identities;
using EventFlow.Core;
using Microsoft.AspNetCore.Mvc.ModelBinding;

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
                throw new ArgumentNullException(nameof(bindingContext));

            var modelName = bindingContext.ModelName;
            var valueProviderResult = bindingContext.ValueProvider.GetValue(modelName);

            if (valueProviderResult == ValueProviderResult.None)
                return Task.CompletedTask;

            bindingContext.ModelState.SetModelValue(modelName, valueProviderResult);

            var identity = valueProviderResult.FirstValue;

            if (!WorkId.IsValid(identity))
            {
                bindingContext.ModelState.TryAddModelError(bindingContext.ModelName, "Not a valid WorkId");
                return Task.CompletedTask;
            }

            var workId = new WorkId(identity);

            bindingContext.Result = ModelBindingResult.Success(workId);
            return Task.CompletedTask;
        }
    }
}