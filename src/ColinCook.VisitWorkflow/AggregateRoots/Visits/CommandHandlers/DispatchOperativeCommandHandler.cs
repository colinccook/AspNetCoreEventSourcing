using ColinCook.VisitWorkflow.AggregateRoots.Visits.Commands;
using ColinCook.VisitWorkflow.Identities;
using EventFlow.Aggregates.ExecutionResults;
using EventFlow.Commands;
using System.Threading;
using System.Threading.Tasks;

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
            IExecutionResult executionResult = aggregate.DispatchOperative(command.OperativeId, command.EstimatedArrival);
            return Task.FromResult(executionResult);
        }
    }
}