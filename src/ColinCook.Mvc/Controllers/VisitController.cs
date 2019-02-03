using System;
using System.Threading;
using System.Threading.Tasks;
using ColinCook.VisitWorkflow.AggregateRoots.Visits.Commands;
using ColinCook.VisitWorkflow.AggregateRoots.Visits.ReadModels;
using ColinCook.VisitWorkflow.Identities;
using EventFlow;
using EventFlow.Aggregates.ExecutionResults;
using EventFlow.Commands;
using EventFlow.Queries;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;

namespace ColinCook.Mvc.Controllers
{
    [Route("visit")]
    public class VisitController : Controller
    {
        private readonly ICommandBus commandBus;
        private readonly IQueryProcessor queryProcessor;

        public VisitController(ICommandBus commandBus, IQueryProcessor queryProcessor)
        {
            this.commandBus = commandBus;
            this.queryProcessor = queryProcessor;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet(nameof(NewVisitId))]
        public IActionResult NewVisitId()
        {
            // Create a new identity for our aggregate root
            var visitId = VisitId.New;

            return new JsonResult(visitId);
        }

        [HttpPost(nameof(AddSite))]
        public async Task<IActionResult> AddSite([FromBody]AddSiteCommand command)
        {
            var executionResult = await commandBus.PublishAsync(
                    command,
                    CancellationToken.None)
                .ConfigureAwait(false);

            return new JsonResult(executionResult);
        }

        [HttpGet(nameof(GetVisit))]
        public async Task<IActionResult> GetVisit(string id)
        {
            // COLIN: TODO: Add custom model binding to pass VisitId straight through... PR?
            var visitReadModel = await queryProcessor.ProcessAsync(
                    new ReadModelByIdQuery<VisitReadModel>(VisitId.With(id)),
                    CancellationToken.None)
                .ConfigureAwait(false);

            return new JsonResult(visitReadModel);
        }
    }
}