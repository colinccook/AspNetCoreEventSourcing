using ColinCook.VisitWorkflow.Identities;
using EventFlow.Aggregates;
using EventFlow.EventStores;
using System.Collections.Generic;

namespace ColinCook.VisitWorkflow.AggregateRoots.Works.Events
{
    [EventVersion(nameof(WorkRaisedEvent), 1)]
    public class WorkRaisedEvent :
        AggregateEvent<WorkAggregate, WorkId>
    {
        public WorkRaisedEvent(string title, string description, IEnumerable<SiteId> sites)
        {
            Title = title;
            Description = description;
            Sites = sites;
        }

        public string Title { get; }
        public string Description { get; }
        public IEnumerable<SiteId> Sites { get; }
    }
}