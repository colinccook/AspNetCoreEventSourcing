using ColinCook.VisitWorkflow.Identities;
using EventFlow.Aggregates.ExecutionResults;
using EventFlow.Commands;
using System.Threading;
using System.Threading.Tasks;

namespace ColinCook.VisitWorkflow.AggregateRoots.Works.Commands
{
    public class WorkAbandonedCommand :
        Command<WorkAggregate, WorkId, IExecutionResult>
    {
        public WorkAbandonedCommand(WorkId workId) : base(workId)
        {
        }
    }

    public class WorkAbandonedCommandHandler :
        CommandHandler<WorkAggregate, WorkId, IExecutionResult, WorkAbandonedCommand>
    {
        public override Task<IExecutionResult> ExecuteCommandAsync(WorkAggregate aggregate,
            WorkAbandonedCommand command, CancellationToken cancellationToken)
        {
            IExecutionResult executionResult = aggregate.Abandon();
            return Task.FromResult(executionResult);
        }
    }
}