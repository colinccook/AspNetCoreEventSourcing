﻿using System.Threading;
using System.Threading.Tasks;
using ColinCCook.AspNetCoreEventSourcing.EventFlow.Identities;
using EventFlow.Aggregates.ExecutionResults;
using EventFlow.Commands;

namespace ColinCCook.AspNetCoreEventSourcing.EventFlow.AggregateRoots.Works.Commands
{
    public class WorkCompletedCommand :
        Command<WorkAggregate, WorkId, IExecutionResult>
    {
        public WorkCompletedCommand(WorkId workId) : base(workId)
        {
        }
    }

    public class WorkCompletedCommandHandler :
        CommandHandler<WorkAggregate, WorkId, IExecutionResult, WorkCompletedCommand>
    {
        public override Task<IExecutionResult> ExecuteCommandAsync(WorkAggregate aggregate,
            WorkCompletedCommand command, CancellationToken cancellationToken)
        {
            var executionResult = aggregate.Complete();
            return Task.FromResult(executionResult);
        }
    }
}