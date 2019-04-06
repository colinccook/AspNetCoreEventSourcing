using System.Threading;
using System.Threading.Tasks;
using ColinCCook.AspNetCoreEventSourcing.EventFlow.AggregateRoots.Visits.Commands;
using ColinCCook.AspNetCoreEventSourcing.EventFlow.Identities;
using EventFlow.Aggregates.ExecutionResults;
using EventFlow.Commands;

namespace ColinCCook.AspNetCoreEventSourcing.EventFlow.AggregateRoots.Visits.CommandHandlers
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
            var executionResult = aggregate.AddSite(command.SiteId);
            return Task.FromResult(executionResult);
        }
    }
}