using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ColinCook.VisitWorkflow.AggregateRoots.Works.Commands;
using ColinCook.VisitWorkflow.Identities;
using EventFlow.Aggregates.ExecutionResults;
using EventFlow.Commands;

namespace ColinCook.VisitWorkflow.AggregateRoots.Works.CommandHandlers
{
    class WorkRaisedCommandHandler :
        CommandHandler<WorkAggregate, WorkId, IExecutionResult, WorkRaisedCommand>
    {
        public override Task<IExecutionResult> ExecuteCommandAsync(WorkAggregate aggregate, WorkRaisedCommand command, CancellationToken cancellationToken)
        {
            var executionResult = aggregate.Raise(command.Title, command.Description, command.Sites);
            return Task.FromResult(executionResult);
        }
    }
}
