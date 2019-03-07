using System.Threading;
using System.Threading.Tasks;
using ColinCook.VisitWorkflow.AggregateRoots.Operatives.Queries;
using ColinCook.VisitWorkflow.AggregateRoots.Works.Commands;
using ColinCook.VisitWorkflow.Identities;
using EventFlow;
using EventFlow.Queries;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ColinCook.RazorPages.Areas.Operatives.Pages
{
    public class OperativeModel : PageModel
    {
        private readonly ICommandBus _commandBus;
        private readonly IQueryProcessor _queryProcessor;

        public OperativeModel(IQueryProcessor queryProcessor, ICommandBus commandBus)
        {
            _queryProcessor = queryProcessor;
            _commandBus = commandBus;
        }

        [BindProperty] public WorkId WorkId { get; set; }
        [BindProperty] public OperativeId OperativeId { get; set; }
        public GetOperativeAndWorkQueryResult QueryResult { get; set; }

        public async Task OnGetAsync(OperativeId operativeId)
        {
            QueryResult = await _queryProcessor.ProcessAsync(
                new GetOperativeAndWorkQuery {OperativeId = operativeId},
                CancellationToken.None);

            if (QueryResult.Work != null)
            {
                WorkId = QueryResult.Work.WorkId;
                OperativeId = QueryResult.Operative.OperativeId;
            }
        }

        public async Task<IActionResult> OnPostAbandonAsync()
        {
            var result = await _commandBus.PublishAsync(new WorkAbandonedCommand(WorkId), CancellationToken.None);

            return RedirectToPage(new {OperativeId});
        }

        public async Task<IActionResult> OnPostCompleteAsync()
        {
            var result = await _commandBus.PublishAsync(new WorkCompletedCommand(WorkId), CancellationToken.None);

            return RedirectToPage(new {OperativeId});
        }
    }
}