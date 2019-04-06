using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ColinCook.RazorPages.Helpers;
using ColinCook.VisitWorkflow.AggregateRoots.Sites.ReadModels;
using ColinCook.VisitWorkflow.AggregateRoots.Works.Commands;
using ColinCook.VisitWorkflow.Identities;
using ColinCook.VisitWorkflow.Queries;
using EventFlow;
using EventFlow.Queries;
using Microsoft.AspNetCore.Mvc;

namespace ColinCook.RazorPages.Areas.Response.Pages
{
    public class IndexModel : BasePageModel
    {
        public IndexModel(IQueryProcessor queryProcessor, ICommandBus commandBus) : base(queryProcessor, commandBus)
        {
        }

        [BindProperty] public string Title { get; set; }
        [BindProperty] public string Description { get; set; }
        public IReadOnlyList<SiteReadModel> Sites { get; set; }

        public async Task OnGetAsync()
        {
            Sites = await QueryProcessor.ProcessAsync(
                new AllSitesQuery(), CancellationToken.None);
        }

        public async Task<IActionResult> OnPostAsync(List<SitePostModel> sites)
        {
            var selectedSites = sites.Where(s => s.IsChecked).Select(s => s.SiteId);

            var result = await CommandBus.PublishAsync(
                new WorkRaisedCommand(WorkId.New, selectedSites, Title, Description), CancellationToken.None);

            return RedirectToPage(result, "raising work");
        }

        public class SitePostModel
        {
            public SiteId SiteId { get; set; }
            public bool IsChecked { get; set; }
        }
    }
}