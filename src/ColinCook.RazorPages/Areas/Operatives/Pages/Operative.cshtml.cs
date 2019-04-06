using System.Threading;
using System.Threading.Tasks;
using ColinCook.RazorPages.Helpers;
using ColinCook.VisitWorkflow.AggregateRoots.Operatives.Queries;
using ColinCook.VisitWorkflow.AggregateRoots.Works.Commands;
using ColinCook.VisitWorkflow.Identities;
using EventFlow;
using EventFlow.Queries;
using Microsoft.AspNetCore.Mvc;

namespace ColinCook.RazorPages.Areas.Operatives.Pages
{
    public class OperativeModel : BasePageModel
    {
        public OperativeModel(IQueryProcessor queryProcessor, ICommandBus commandBus) : base(queryProcessor, commandBus)
        {
        }

        [BindProperty] public WorkId WorkId { get; set; }
        [BindProperty] public OperativeId OperativeId { get; set; }
        public GetOperativeAndWorkQueryResult QueryResult { get; set; }

        public async Task OnGetAsync(OperativeId operativeId)
        {
            QueryResult = await QueryProcessor.ProcessAsync(
                new GetOperativeAndWorkQuery { OperativeId = operativeId },
                CancellationToken.None);

            if (QueryResult.Work != null)
            {
                WorkId = QueryResult.Work.WorkId;
                OperativeId = QueryResult.Operative.OperativeId;
            }
        }

        public async Task<IActionResult> OnPostAbandonAsync()
        {
            var result = await CommandBus.PublishAsync(new WorkAbandonedCommand(WorkId), CancellationToken.None);

            return RedirectToPage(new { OperativeId }, result, "abandoning work");
        }

        public async Task<IActionResult> OnPostCompleteAsync()
        {
            var result = await CommandBus.PublishAsync(new WorkCompletedCommand(WorkId), CancellationToken.None);

            return RedirectToPage(new { OperativeId }, result, "completing work");
        }
    }
}