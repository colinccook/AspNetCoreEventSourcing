using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ColinCCook.AspNetCoreEventSourcing.EventFlow.AggregateRoots.Sites.Commands;
using ColinCCook.AspNetCoreEventSourcing.EventFlow.AggregateRoots.Sites.ReadModels;
using ColinCCook.AspNetCoreEventSourcing.EventFlow.Identities;
using ColinCCook.AspNetCoreEventSourcing.EventFlow.Queries;
using ColinCCook.AspNetCoreEventSourcing.RazorPages.Helpers;
using EventFlow;
using EventFlow.Queries;
using Microsoft.AspNetCore.Mvc;

namespace ColinCCook.AspNetCoreEventSourcing.RazorPages.Areas.Administration.Pages
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