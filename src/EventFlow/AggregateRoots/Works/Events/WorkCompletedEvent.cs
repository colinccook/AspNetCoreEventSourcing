using ColinCCook.AspNetCoreEventSourcing.EventFlow.Identities;
using EventFlow.Aggregates;
using EventFlow.EventStores;

namespace ColinCCook.AspNetCoreEventSourcing.EventFlow.AggregateRoots.Works.Events
{
    [EventVersion(nameof(WorkCompletedEvent), 1)]
    public class WorkCompletedEvent :
        AggregateEvent<WorkAggregate, WorkId>
    {
    }
}