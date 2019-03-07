using EventFlow.Aggregates;
using EventFlow.Aggregates.ExecutionResults;

namespace EventFlow.Documentation.Topics.Commands
{
    public class UserAggregate : AggregateRoot<UserAggregate, UserId>
    {
        public UserAggregate(UserId id) : base(id)
        {
        }

        public IExecutionResult UpdatePassword(
            Password newPassword,
            Password oldPassword)
        {
            // Dummy method
            return ExecutionResult.Success();
        }
    }
}