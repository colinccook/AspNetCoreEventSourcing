using System.Threading;
using System.Threading.Tasks;
using ColinCook.VisitWorkflow.AggregateRoots.Sites.Commands;
using ColinCook.VisitWorkflow.Identities;
using EventFlow.Aggregates.ExecutionResults;
using EventFlow.Commands;

namespace ColinCook.VisitWorkflow.AggregateRoots.Sites.CommandHandlers
{
    public class SiteAcquiredCommandHandler :
        CommandHandler<SiteAggregate, SiteId, IExecutionResult, SiteAcquiredCommand>
    {
        public override Task<IExecutionResult> ExecuteCommandAsync(SiteAggregate aggregate, SiteAcquiredCommand command,
            CancellationToken cancellationToken)
        {
            var executionResult = aggregate.Acquire(command.AddressLine1, command.Town, command.PostCode, command.TelephoneNumber);
            return Task.FromResult(executionResult);
        }
    }
}
