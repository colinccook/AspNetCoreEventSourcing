using ColinCook.VisitWorkflow.Identities;
using EventFlow.Aggregates;
using EventFlow.EventStores;

namespace ColinCook.VisitWorkflow.AggregateRoots.Visits.Events
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