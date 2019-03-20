using ColinCook.VisitWorkflow.AggregateRoots.Visits.Commands;
using ColinCook.VisitWorkflow.Identities;
using EventFlow.Aggregates.ExecutionResults;
using EventFlow.Commands;
using System.Threading;
using System.Threading.Tasks;

namespace ColinCook.VisitWorkflow.AggregateRoots.Visits.CommandHandlers
{
    /// Command handler for our command
    public class AddSiteCommandHandler :
        CommandHandler<VisitAggregate, VisitId, IExecutionResult, AddSiteCommand>
    {
        public override Task<IExecutionResult> ExecuteCommandAsync(
            VisitAggregate aggregate,
            AddSiteCommand command,
            CancellationToken cancellationToken)
        {
            IExecutionResult executionResult = aggregate.AddSite(command.SiteId);
            return Task.FromResult(executionResult);
        }
    }
}