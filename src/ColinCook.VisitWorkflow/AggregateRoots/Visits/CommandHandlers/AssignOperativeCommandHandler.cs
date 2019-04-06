using System.Threading;
using System.Threading.Tasks;
using ColinCook.VisitWorkflow.AggregateRoots.Visits.Commands;
using ColinCook.VisitWorkflow.Identities;
using EventFlow.Aggregates.ExecutionResults;
using EventFlow.Commands;

namespace ColinCook.VisitWorkflow.AggregateRoots.Visits.CommandHandlers
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