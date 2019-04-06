using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ColinCook.RazorPages.Helpers;
using ColinCook.VisitWorkflow.AggregateRoots.Operatives.ReadModels;
using ColinCook.VisitWorkflow.Queries;
using EventFlow;
using EventFlow.Queries;

namespace ColinCook.RazorPages.Areas.Operatives.Pages
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
                new AllOperativesQuery(), CancellationToken.None);
        }
    }
}