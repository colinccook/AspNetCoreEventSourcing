using System.Threading;
using System.Threading.Tasks;
using ColinCCook.AspNetCoreEventSourcing.EventFlow.Identities;
using EventFlow.Aggregates.ExecutionResults;
using EventFlow.Commands;

namespace ColinCCook.AspNetCoreEventSourcing.EventFlow.AggregateRoots.Works.Commands
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
            var executionResult = aggregate.Abandon();
            return Task.FromResult(executionResult);
        }
    }
}