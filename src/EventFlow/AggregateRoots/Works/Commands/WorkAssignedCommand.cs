﻿using System.Threading;
using System.Threading.Tasks;
using ColinCCook.AspNetCoreEventSourcing.EventFlow.Identities;
using EventFlow.Aggregates.ExecutionResults;
using EventFlow.Commands;

namespace ColinCCook.AspNetCoreEventSourcing.EventFlow.AggregateRoots.Works.Commands
{
    public class WorkAssignedCommand :
        Command<WorkAggregate, WorkId, IExecutionResult>
    {
        public WorkAssignedCommand(WorkId workId, OperativeId operativeId) : base(workId)
        {
            OperativeId = operativeId;
        }

        public OperativeId OperativeId { get; }
    }

    public class WorkAssignedCommandHandler :
        CommandHandler<WorkAggregate, WorkId, IExecutionResult, WorkAssignedCommand>
    {
        public override Task<IExecutionResult> ExecuteCommandAsync(WorkAggregate aggregate, WorkAssignedCommand command,
            CancellationToken cancellationToken)
        {
            var executionResult = aggregate.Assign(command.OperativeId);
            return Task.FromResult(executionResult);
        }
    }
}