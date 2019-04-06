using ColinCCook.AspNetCoreEventSourcing.EventFlow.AggregateRoots.Sites.Events;
using ColinCCook.AspNetCoreEventSourcing.EventFlow.Identities;
using EventFlow.Aggregates;
using EventFlow.Aggregates.ExecutionResults;

namespace ColinCCook.AspNetCoreEventSourcing.EventFlow.AggregateRoots.Sites
{
    /// The aggregate root
    public class SiteAggregate :
        AggregateRoot<SiteAggregate, SiteId>,
        IEmit<SiteAcquiredEvent>
    {
        private string _addressLine1;
        private string _postCode;
        private string _telephoneNumber;
        private string _town;

        public SiteAggregate(SiteId id) : base(id)
        {
        }

        public void Apply(SiteAcquiredEvent aggregateEvent)
        {
            _addressLine1 = aggregateEvent.AddressLine1;
            _town = aggregateEvent.Town;
            _postCode = aggregateEvent.PostCode;
            _telephoneNumber = aggregateEvent.TelephoneNumber;
        }

        internal IExecutionResult Acquire(string addressLine1, string town, string postCode, string telephoneNumber)
        {
            if (string.IsNullOrWhiteSpace(addressLine1)) return ExecutionResult.Failed("Site must have an address line 1");

            if (string.IsNullOrWhiteSpace(town)) return ExecutionResult.Failed("Site must have a town");

            if (string.IsNullOrWhiteSpace(postCode)) return ExecutionResult.Failed("Site must have a post code");

            if (string.IsNullOrWhiteSpace(telephoneNumber)) return ExecutionResult.Failed("Site must have a telephone number");

            Emit(new SiteAcquiredEvent(addressLine1, town, postCode, telephoneNumber));

            return ExecutionResult.Success();
        }
    }
}