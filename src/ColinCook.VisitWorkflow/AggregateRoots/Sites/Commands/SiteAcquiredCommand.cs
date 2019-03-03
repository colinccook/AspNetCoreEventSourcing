using System.Threading;
using System.Threading.Tasks;
using ColinCook.VisitWorkflow.Identities;
using EventFlow.Aggregates.ExecutionResults;
using EventFlow.Commands;

namespace ColinCook.VisitWorkflow.AggregateRoots.Sites.Commands
{
    public class SiteAcquiredCommand :
        Command<SiteAggregate, SiteId, IExecutionResult>
    {
        public SiteAcquiredCommand(SiteId siteId, string addressLine1, string town, string postCode, string telephoneNumber) : base(siteId)
        {
            AddressLine1 = addressLine1;
            Town = town;
            PostCode = postCode;
            TelephoneNumber = telephoneNumber;
        }

        public string AddressLine1 { get; }
        public string Town { get; }
        public string PostCode { get; }
        public string TelephoneNumber { get; }
    }

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
