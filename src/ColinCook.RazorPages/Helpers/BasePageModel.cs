using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ColinCook.RazorPages.Extensions;
using EventFlow.Aggregates.ExecutionResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ColinCook.RazorPages.Helpers
{
    public abstract class BasePageModel : PageModel
    {
        public IActionResult RedirectToPage(IExecutionResult executionResult, string successMessage = "")
        {
            if (executionResult.IsSuccess)
                TempData.Set("SuccessMessage", string.IsNullOrEmpty(successMessage) ? "Completed successfully" : successMessage);
            else
                TempData.Set("FailMessage", "Could not complete the operation");

            return RedirectToPage();
        }
    }
}
