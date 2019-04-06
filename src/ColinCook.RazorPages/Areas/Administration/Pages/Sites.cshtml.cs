using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ColinCook.RazorPages.Helpers;
using ColinCook.VisitWorkflow.AggregateRoots.Sites.Commands;
using ColinCook.VisitWorkflow.AggregateRoots.Sites.ReadModels;
using ColinCook.VisitWorkflow.Identities;
using ColinCook.VisitWorkflow.Queries;
using EventFlow;
using EventFlow.Queries;
using Microsoft.AspNetCore.Mvc;

namespace ColinCook.RazorPages.Areas.Administration.Pages
{
    public class SitesModel : BasePageModel
    {
        public SitesModel(IQueryProcessor queryProcessor, ICommandBus commandBus) : base(queryProcessor, commandBus)
        {
        }

        [BindProperty] public string AddressLine1 { get; set; }
        [BindProperty] public string Town { get; set; }
        [BindProperty] public string PostCode { get; set; }
        [BindProperty] public string TelephoneNumber { get; set; }

        public IReadOnlyList<SiteReadModel> Sites { get; set; }

        public async Task OnGetAsync()
        {
            Sites = await QueryProcessor.ProcessAsync(
                new AllSitesQuery(), CancellationToken.None);
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var result = await CommandBus.PublishAsync(
                new SiteAcquiredCommand(SiteId.New, AddressLine1, Town, PostCode, TelephoneNumber),
                CancellationToken.None);

            return RedirectToPage(result, "acquiring Site");
        }
    }
}