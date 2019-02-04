using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ColinCook.VisitWorkflow.AggregateRoots.Operatives.Queries;
using EventFlow.Queries;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ColinCook.RazorPages.Areas.Administration.Pages
{
    public class OperativesModel : PageModel
    {
        private readonly IQueryProcessor _queryProcessor;

        public OperativesModel(IQueryProcessor queryProcessor)
        {
            _queryProcessor = queryProcessor;
        }

        public void OnGet()
        {
            var allOperatives = _queryProcessor.Process(
                new AllOperativesQuery(), CancellationToken.None);
        }
    }
}