using System.Threading;
using System.Threading.Tasks;
using ColinCCook.AspNetCoreEventSourcing.EventFlow.AggregateRoots.Works.Commands;
using ColinCCook.AspNetCoreEventSourcing.EventFlow.Identities;
using ColinCCook.AspNetCoreEventSourcing.EventFlow.Queries;
using ColinCCook.AspNetCoreEventSourcing.RazorPages.Helpers;
using EventFlow;
using EventFlow.Queries;
using Microsoft.AspNetCore.Mvc;

namespace ColinCCook.AspNetCoreEventSourcing.RazorPages.Areas.Control.Pages
{
    public class IndexModel : BasePageModel
    {
        public IndexModel(IQueryProcessor queryProcessor, ICommandBus commandBus) : base(queryProcessor, commandBus)
        {
        }

        public GetLatestWorkToAssignResult GetLatestWorkToAssign { get; set; }

        public async Task OnGetAsync()
        {
            GetLatestWorkToAssign = await QueryProcessor.ProcessAsync(
                new GetLatestWorkToAssignQuery(), CancellationToken.None);
        }

        public async Task<IActionResult> OnPostAsync(WorkId workId, OperativeId operativeId)
        {
            var result =
                await CommandBus.PublishAsync(new WorkAssignedCommand(workId, operativeId), CancellationToken.None);

            return RedirectToPage(result, "assigning work");
        }
    }
}