using ColinCCook.AspNetCoreEventSourcing.RazorPages.Extensions;
using EventFlow;
using EventFlow.Aggregates.ExecutionResults;
using EventFlow.Queries;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ColinCCook.AspNetCoreEventSourcing.RazorPages.Helpers
{
    public abstract class BasePageModel : PageModel
    {
        protected BasePageModel(IQueryProcessor queryProcessor, ICommandBus commandBus)
        {
            QueryProcessor = queryProcessor;
            CommandBus = commandBus;
        }

        protected ICommandBus CommandBus { get; }
        protected IQueryProcessor QueryProcessor { get; }

        /// <summary>
        ///     Redirects with an execution result
        /// </summary>
        /// <param name="executionResult"></param>
        /// <param name="attemptedAction">Should be worded like 'hiring an operative'. It is prefixed with Succeeded or Failed</param>
        /// <returns></returns>
        public IActionResult RedirectToPage(IExecutionResult executionResult, string attemptedAction = "")
        {
            if (executionResult is FailedExecutionResult failedExecutionResult)
            {
                TempData.Set("FailMessage", $"Could not complete action: {attemptedAction}");
                TempData.Set("FailReason", failedExecutionResult.ToString());
            }
            else
            {
                TempData.Set("SuccessMessage", $"Succeeded {attemptedAction}");
            }

            return RedirectToPage();
        }

        public IActionResult RedirectToPage(object routeValues, IExecutionResult executionResult, string attemptedAction = "")
        {
            if (executionResult is FailedExecutionResult failedExecutionResult)
            {
                TempData.Set("FailMessage", $"Could not complete action: {attemptedAction}");
                TempData.Set("FailReason", failedExecutionResult.ToString());
            }
            else
            {
                TempData.Set("SuccessMessage", $"Succeeded {attemptedAction}");
            }

            return RedirectToPage(routeValues);
        }
    }
}