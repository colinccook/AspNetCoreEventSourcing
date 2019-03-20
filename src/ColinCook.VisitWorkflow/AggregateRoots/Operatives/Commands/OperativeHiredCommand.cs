using ColinCook.VisitWorkflow.Identities;
using EventFlow.Aggregates.ExecutionResults;
using EventFlow.Commands;
using System.Threading;
using System.Threading.Tasks;

namespace ColinCook.VisitWorkflow.AggregateRoots.Operatives.Commands
{
    public class OperativeHiredCommand :
        Command<OperativeAggregate, OperativeId, IExecutionResult>
    {
        public OperativeHiredCommand(OperativeId operativeId, string forename, string surname) : base(operativeId)
        {
            Forename = forename;
            Surname = surname;
        }

        public string Forename { get; }
        public string Surname { get; }
    }

    public class OperativeHiredCommandHandler :
        CommandHandler<OperativeAggregate, OperativeId, IExecutionResult, OperativeHiredCommand>
    {
        public override Task<IExecutionResult> ExecuteCommandAsync(OperativeAggregate aggregate,
            OperativeHiredCommand command,
            CancellationToken cancellationToken)
        {
            IExecutionResult executionResult = aggregate.Hire(command.Forename, command.Surname);
            return Task.FromResult(executionResult);
        }
    }
}