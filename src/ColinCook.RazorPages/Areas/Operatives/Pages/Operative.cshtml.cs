using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ColinCook.VisitWorkflow.AggregateRoots.Operatives.Queries;
using ColinCook.VisitWorkflow.Identities;
using EventFlow;
using EventFlow.Queries;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ColinCook.RazorPages.Areas.Operatives.Pages
{
    public class OperativeModel : PageModel
    {
        private readonly IQueryProcessor _queryProcessor;
        private readonly ICommandBus _commandBus;

        public GetOperativeAndWorkQueryResult Query { get; set; }

        public OperativeModel(IQueryProcessor queryProcessor, ICommandBus commandBus)
        {
            _queryProcessor = queryProcessor;
            _commandBus = commandBus;
        }

        public async Task OnGetAsync(OperativeId operativeId)
        {
            Query = await _queryProcessor.ProcessAsync(
                new GetOperativeAndWorkQuery {OperativeId = operativeId},
                CancellationToken.None);
        }
    }
}