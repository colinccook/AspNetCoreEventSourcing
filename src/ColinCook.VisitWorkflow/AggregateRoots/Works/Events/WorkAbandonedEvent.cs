using ColinCook.VisitWorkflow.Identities;
using EventFlow.Aggregates;
using EventFlow.EventStores;

namespace ColinCook.VisitWorkflow.AggregateRoots.Works.Events
{
    [EventVersion(nameof(WorkAbandonedEvent), 1)]
    public class WorkAbandonedEvent :
        AggregateEvent<WorkAggregate, WorkId>
    {
    }
}