using System.Threading;
using System.Threading.Tasks;
using EventFlow.Aggregates.ExecutionResults;
using EventFlow.Commands;

namespace EventFlow.Documentation.Topics.Commands
{
    public class UserUpdatePasswordCommandHandler :
        CommandHandler<UserAggregate, UserId, IExecutionResult, UserUpdatePasswordCommand>
    {
        public override Task<IExecutionResult> ExecuteCommandAsync(
            UserAggregate aggregate,
            UserUpdatePasswordCommand command,
            CancellationToken cancellationToken)
        {
            var executionResult = aggregate.UpdatePassword(
                command.OldPassword,
                command.NewPassword);

            return Task.FromResult(executionResult);
        }
    }
}