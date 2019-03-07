using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ColinCook.VisitWorkflow.AggregateRoots.Operatives.Queries;
using ColinCook.VisitWorkflow.AggregateRoots.Operatives.ReadModels;
using EventFlow;
using EventFlow.Queries;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ColinCook.RazorPages.Areas.Operatives.Pages
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

        public IReadOnlyList<OperativeReadModel> Operatives { get; set; }

        public async Task OnGetAsync()
        {
            Operatives = await _queryProcessor.ProcessAsync(
                new AllOperativesQuery(), CancellationToken.None);
        }
    }
}