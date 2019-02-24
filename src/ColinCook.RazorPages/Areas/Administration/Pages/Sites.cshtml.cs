using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ColinCook.VisitWorkflow.AggregateRoots.Operatives.Commands;
using ColinCook.VisitWorkflow.AggregateRoots.Operatives.Queries;
using ColinCook.VisitWorkflow.AggregateRoots.Sites.Commands;
using ColinCook.VisitWorkflow.AggregateRoots.Sites.Queries;
using ColinCook.VisitWorkflow.AggregateRoots.Sites.ReadModels;
using ColinCook.VisitWorkflow.Identities;
using EventFlow;
using EventFlow.Extensions;
using EventFlow.Queries;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ColinCook.RazorPages.Areas.Administration.Pages
{
    public class SitesModel : PageModel
    {
        private readonly IQueryProcessor _queryProcessor;
        private readonly ICommandBus _commandBus;

        [BindProperty] public string AddressLine1 { get; set; }
        [BindProperty] public string Town { get; set; }
        [BindProperty] public string PostCode { get; set; }
        [BindProperty] public string TelephoneNumber { get; set; }

        public IReadOnlyList<SiteReadModel> Sites { get; set; }

        public SitesModel(IQueryProcessor queryProcessor, ICommandBus commandBus)
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
            var result = await _commandBus.PublishAsync(new SiteAcquiredCommand(SiteId.New, AddressLine1, Town, PostCode, TelephoneNumber), CancellationToken.None);

            return RedirectToPage();
        }
    }
}