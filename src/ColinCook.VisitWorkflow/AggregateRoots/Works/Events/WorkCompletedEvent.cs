using ColinCook.VisitWorkflow.Identities;
using EventFlow.Aggregates;
using EventFlow.EventStores;

namespace ColinCook.VisitWorkflow.AggregateRoots.Works.Events
{
    [EventVersion(nameof(WorkCompletedEvent), 1)]
    public class WorkCompletedEvent :
        AggregateEvent<WorkAggregate, WorkId>
    {
    }
}