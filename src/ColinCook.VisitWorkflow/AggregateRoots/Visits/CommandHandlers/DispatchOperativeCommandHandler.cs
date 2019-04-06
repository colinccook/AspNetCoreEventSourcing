using System.Threading;
using System.Threading.Tasks;
using ColinCook.VisitWorkflow.AggregateRoots.Visits.Commands;
using ColinCook.VisitWorkflow.Identities;
using EventFlow.Aggregates.ExecutionResults;
using EventFlow.Commands;

namespace ColinCook.VisitWorkflow.AggregateRoots.Visits.CommandHandlers
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