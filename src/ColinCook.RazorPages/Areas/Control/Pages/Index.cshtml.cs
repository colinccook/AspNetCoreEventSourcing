using ColinCook.RazorPages.Helpers;
using ColinCook.VisitWorkflow.AggregateRoots.Works.Commands;
using ColinCook.VisitWorkflow.AggregateRoots.Works.Queries;
using ColinCook.VisitWorkflow.Identities;
using EventFlow;
using EventFlow.Queries;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;

namespace ColinCook.RazorPages.Areas.Control.Pages
{
    public class IndexModel : BasePageModel
    {
        public AssignWorkQueryResult AssignWorkQuery { get; set; }

        public async Task OnGetAsync()
        {
            AssignWorkQuery = await QueryProcessor.ProcessAsync(
                new AssignWorkQuery(), CancellationToken.None);
        }

        public async Task<IActionResult> OnPostAsync(WorkId workId, OperativeId operativeId)
        {
            EventFlow.Aggregates.ExecutionResults.IExecutionResult result =
                await CommandBus.PublishAsync(new WorkAssignedCommand(workId, operativeId), CancellationToken.None);

            return RedirectToPage(result, "assigning work");
        }

        public IndexModel(IQueryProcessor queryProcessor, ICommandBus commandBus) : base(queryProcessor, commandBus)
        {
        }
    }
}