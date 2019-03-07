using System.Threading;
using System.Threading.Tasks;
using ColinCook.VisitWorkflow.AggregateRoots.Works.Commands;
using ColinCook.VisitWorkflow.AggregateRoots.Works.Queries;
using ColinCook.VisitWorkflow.Identities;
using EventFlow;
using EventFlow.Queries;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ColinCook.RazorPages.Areas.Control.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ICommandBus _commandBus;
        private readonly IQueryProcessor _queryProcessor;

        public IndexModel(IQueryProcessor queryProcessor, ICommandBus commandBus)
        {
            _queryProcessor = queryProcessor;
            _commandBus = commandBus;
        }

        public AssignWorkQueryResult AssignWorkQuery { get; set; }

        public async Task OnGetAsync()
        {
            AssignWorkQuery = await _queryProcessor.ProcessAsync(
                new AssignWorkQuery(), CancellationToken.None);
        }

        public async Task<IActionResult> OnPostAsync(WorkId workId, OperativeId operativeId)
        {
            var result =
                await _commandBus.PublishAsync(new WorkAssignedCommand(workId, operativeId), CancellationToken.None);

            return RedirectToPage();
        }
    }
}