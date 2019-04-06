using System.Threading;
using System.Threading.Tasks;
using EventFlow.Aggregates.ExecutionResults;
using EventFlow.Commands;

namespace ColinCook.Documentation.GettingStarted
{
    /// Command handler for our command
    public class ExampleCommandHandler :
        CommandHandler<ExampleAggregate, ExampleId, IExecutionResult, ExampleCommand>
    {
        public override Task<IExecutionResult> ExecuteCommandAsync(
            ExampleAggregate aggregate,
            ExampleCommand command,
            CancellationToken cancellationToken)
        {
            var executionResult = aggregate.SetMagicNumer(command.MagicNumber);
            return Task.FromResult(executionResult);
        }
    }
}