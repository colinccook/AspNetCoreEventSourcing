using ColinCook.RazorPages.Helpers;
using ColinCook.VisitWorkflow.AggregateRoots.Operatives.Queries;
using ColinCook.VisitWorkflow.AggregateRoots.Operatives.ReadModels;
using EventFlow;
using EventFlow.Queries;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ColinCook.RazorPages.Areas.Operatives.Pages
{
    public class IndexModel : BasePageModel
    {
        public IReadOnlyList<OperativeReadModel> Operatives { get; set; }

        public async Task OnGetAsync()
        {
            Operatives = await QueryProcessor.ProcessAsync(
                new AllOperativesQuery(), CancellationToken.None);
        }

        public IndexModel(IQueryProcessor queryProcessor, ICommandBus commandBus) : base(queryProcessor, commandBus)
        {
        }
    }
}