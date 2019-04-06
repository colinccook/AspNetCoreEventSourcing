using ColinCCook.AspNetCoreEventSourcing.EventFlow.Identities;
using EventFlow.Aggregates;
using EventFlow.EventStores;

namespace ColinCCook.AspNetCoreEventSourcing.EventFlow.AggregateRoots.Visits.Events
{
    /// A basic event containing some information
    [EventVersion(nameof(AddSiteEvent), 1)]
    public class AddSiteEvent :
        AggregateEvent<VisitAggregate, VisitId>
    {
        public AddSiteEvent(SiteId siteId)
        {
            SiteId = siteId;
        }

        public SiteId SiteId { get; }
    }
}