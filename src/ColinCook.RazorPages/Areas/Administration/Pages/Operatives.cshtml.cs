using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ColinCook.RazorPages.Helpers;
using ColinCook.VisitWorkflow.AggregateRoots.Operatives.Commands;
using ColinCook.VisitWorkflow.AggregateRoots.Operatives.ReadModels;
using ColinCook.VisitWorkflow.Identities;
using ColinCook.VisitWorkflow.Queries;
using EventFlow;
using EventFlow.Queries;
using Microsoft.AspNetCore.Mvc;

namespace ColinCook.RazorPages.Areas.Administration.Pages
{
    public class OperativesModel : BasePageModel
    {
        public OperativesModel(IQueryProcessor queryProcessor, ICommandBus commandBus) : base(queryProcessor, commandBus)
        {
        }

        [BindProperty] public string Forename { get; set; }
        [BindProperty] public string Surname { get; set; }

        public IReadOnlyList<OperativeReadModel> Operatives { get; set; }

        public async Task OnGetAsync()
        {
            Operatives = await QueryProcessor.ProcessAsync(
                new AllOperativesQuery(), CancellationToken.None);
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var result = await CommandBus.PublishAsync(new OperativeHiredCommand(OperativeId.New, Forename, Surname),
                CancellationToken.None);

            return RedirectToPage(result, "hiring an Operative");
        }
    }
}