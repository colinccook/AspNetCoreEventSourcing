using ColinCook.VisitWorkflow.AggregateRoots.Sites.Models;
using ColinCook.VisitWorkflow.AggregateRoots.Sites.Queries;
using ColinCook.VisitWorkflow.Identities;
using EventFlow;
using EventFlow.Queries;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ColinCook.VisitWorkflow.AggregateRoots.Works.Commands;
using Microsoft.AspNetCore.Mvc;

namespace ColinCook.RazorPages.Areas.Response.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IQueryProcessor _queryProcessor;
        private readonly ICommandBus _commandBus;

        [BindProperty] public string Title { get; set; }
        [BindProperty] public string Description { get; set; }
        [BindProperty] public IReadOnlyList<SiteModel> Sites { get; set; }

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

        public async Task<IActionResult> OnPostAsync()
        {
            var selectedSites = Sites.Where(s => s.IsChecked).Select(s => s.SiteId);

            var result = await _commandBus.PublishAsync(new WorkRaisedCommand(WorkId.New, selectedSites, Title, Description), CancellationToken.None);

            return RedirectToPage();
        }
    }
}