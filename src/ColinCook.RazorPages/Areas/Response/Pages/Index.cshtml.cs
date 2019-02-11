using ColinCook.VisitWorkflow.AggregateRoots.Sites.Models;
using ColinCook.VisitWorkflow.AggregateRoots.Sites.Queries;
using ColinCook.VisitWorkflow.Identities;
using EventFlow;
using EventFlow.Queries;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace ColinCook.RazorPages.Areas.Response.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IQueryProcessor _queryProcessor;
        private readonly ICommandBus _commandBus;

        public IReadOnlyCollection<SiteModel> Sites { get; set; }

        public IndexModel(IQueryProcessor queryProcessor, ICommandBus commandBus)
        {
            _queryProcessor = queryProcessor;
            _commandBus = commandBus;
        }

        public async Task OnGetAsync()
        {
            Sites = await _queryProcessor.ProcessAsync(
                new AllSitesQuery(), CancellationToken.None);
        }
    }
}