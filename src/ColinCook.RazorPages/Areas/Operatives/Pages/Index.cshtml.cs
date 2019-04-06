using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ColinCCook.AspNetCoreEventSourcing.EventFlow.AggregateRoots.Operatives.ReadModels;
using ColinCCook.AspNetCoreEventSourcing.EventFlow.Queries;
using ColinCCook.AspNetCoreEventSourcing.RazorPages.Helpers;
using EventFlow;
using EventFlow.Queries;

namespace ColinCCook.AspNetCoreEventSourcing.RazorPages.Areas.Operatives.Pages
{
    public class IndexModel : BasePageModel
    {
        public IndexModel(IQueryProcessor queryProcessor, ICommandBus commandBus) : base(queryProcessor, commandBus)
        {
        }

        public IReadOnlyList<OperativeReadModel> Operatives { get; set; }

        public async Task OnGetAsync()
        {
            Operatives = await QueryProcessor.ProcessAsync(
                new GetAllOperativesQuery(), CancellationToken.None);
        }
    }
}