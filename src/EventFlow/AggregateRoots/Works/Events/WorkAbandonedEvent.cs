using ColinCCook.AspNetCoreEventSourcing.EventFlow.Identities;
using EventFlow.Aggregates;
using EventFlow.EventStores;

namespace ColinCCook.AspNetCoreEventSourcing.EventFlow.AggregateRoots.Works.Events
{
    [EventVersion(nameof(WorkAbandonedEvent), 1)]
    public class WorkAbandonedEvent :
        AggregateEvent<WorkAggregate, WorkId>
    {
    }
}