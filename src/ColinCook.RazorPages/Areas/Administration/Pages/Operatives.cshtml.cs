using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ColinCook.VisitWorkflow.AggregateRoots.Operatives.Commands;
using ColinCook.VisitWorkflow.AggregateRoots.Operatives.Queries;
using ColinCook.VisitWorkflow.AggregateRoots.Operatives.ReadModels;
using ColinCook.VisitWorkflow.Identities;
using EventFlow;
using EventFlow.Queries;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ColinCook.RazorPages.Areas.Administration.Pages
{
    public class OperativesModel : PageModel
    {
        private readonly ICommandBus _commandBus;
        private readonly IQueryProcessor _queryProcessor;

        public OperativesModel(IQueryProcessor queryProcessor, ICommandBus commandBus)
        {
            _queryProcessor = queryProcessor;
            _commandBus = commandBus;
        }

        [BindProperty] public string Forename { get; set; }
        [BindProperty] public string Surname { get; set; }

        public IReadOnlyList<OperativeReadModel> Operatives { get; set; }

        public async Task OnGetAsync()
        {
            Operatives = await _queryProcessor.ProcessAsync(
                new AllOperativesQuery(), CancellationToken.None);
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var result = await _commandBus.PublishAsync(new OperativeHiredCommand(OperativeId.New, Forename, Surname),
                CancellationToken.None);

            return RedirectToPage();
        }
    }
}