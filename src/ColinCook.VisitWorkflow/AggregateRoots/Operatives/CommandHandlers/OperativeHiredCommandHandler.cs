using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ColinCook.VisitWorkflow.AggregateRoots.Operatives.Commands;
using ColinCook.VisitWorkflow.Identities;
using EventFlow.Aggregates.ExecutionResults;
using EventFlow.Commands;

namespace ColinCook.VisitWorkflow.AggregateRoots.Operatives.CommandHandlers
{
    public class OperativeHiredCommandHandler :
        CommandHandler<OperativeAggregate, OperativeId, IExecutionResult, OperativeHiredCommand>
    {
        public override Task<IExecutionResult> ExecuteCommandAsync(OperativeAggregate aggregate, OperativeHiredCommand command,
            CancellationToken cancellationToken)
        {
            var executionResult = aggregate.Hire(command.Forename, command.Surname);
            return Task.FromResult(executionResult);
        }
    }
}
