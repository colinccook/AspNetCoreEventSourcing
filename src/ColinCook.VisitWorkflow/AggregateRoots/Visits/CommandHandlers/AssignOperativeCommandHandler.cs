using System.Threading;
using System.Threading.Tasks;
using ColinCCook.AspNetCoreEventSourcing.EventFlow.AggregateRoots.Visits.Commands;
using ColinCCook.AspNetCoreEventSourcing.EventFlow.Identities;
using EventFlow.Aggregates.ExecutionResults;
using EventFlow.Commands;

namespace ColinCCook.AspNetCoreEventSourcing.EventFlow.AggregateRoots.Visits.CommandHandlers
{
    /// Command handler for our command
    public class AssignOperativeCommandHandler :
        CommandHandler<VisitAggregate, VisitId, IExecutionResult, AssignOperativeCommand>
    {
        public override Task<IExecutionResult> ExecuteCommandAsync(
            VisitAggregate aggregate,
            AssignOperativeCommand command,
            CancellationToken cancellationToken)
        {
            var executionResult = aggregate.AssignOperative(command.OperativeId);
            return Task.FromResult(executionResult);
        }
    }
}