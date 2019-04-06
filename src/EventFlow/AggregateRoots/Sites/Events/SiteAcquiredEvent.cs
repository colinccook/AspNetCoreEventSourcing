using ColinCCook.AspNetCoreEventSourcing.EventFlow.Identities;
using EventFlow.Aggregates;
using EventFlow.EventStores;

namespace ColinCCook.AspNetCoreEventSourcing.EventFlow.AggregateRoots.Sites.Events
{
    [EventVersion(nameof(SiteAcquiredEvent), 1)]
    public class SiteAcquiredEvent : AggregateEvent<SiteAggregate, SiteId>
    {
        public SiteAcquiredEvent(string addressLine1, string town, string postCode, string telephoneNumber)
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
}