using System.Threading;
using System.Threading.Tasks;
using ColinCCook.AspNetCoreEventSourcing.EventFlow.AggregateRoots.Visits.Commands;
using ColinCCook.AspNetCoreEventSourcing.EventFlow.Identities;
using EventFlow.Aggregates.ExecutionResults;
using EventFlow.Commands;

namespace ColinCCook.AspNetCoreEventSourcing.EventFlow.AggregateRoots.Visits.CommandHandlers
{
    /// Command handler for our command
    public class DispatchOperativeCommandHandler :
        CommandHandler<VisitAggregate, VisitId, IExecutionResult, DispatchOperativeCommand>
    {
        public override Task<IExecutionResult> ExecuteCommandAsync(
            VisitAggregate aggregate,
            DispatchOperativeCommand command,
            CancellationToken cancellationToken)
        {
            var executionResult = aggregate.DispatchOperative(command.OperativeId, command.EstimatedArrival);
            return Task.FromResult(executionResult);
        }
    }
}