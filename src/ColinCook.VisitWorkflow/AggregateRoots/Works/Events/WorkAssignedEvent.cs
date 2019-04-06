using ColinCCook.AspNetCoreEventSourcing.EventFlow.Identities;
using EventFlow.Aggregates;
using EventFlow.EventStores;

namespace ColinCCook.AspNetCoreEventSourcing.EventFlow.AggregateRoots.Works.Events
{
    [EventVersion(nameof(WorkAssignedEvent), 1)]
    public class WorkAssignedEvent :
        AggregateEvent<WorkAggregate, WorkId>
    {
        public WorkAssignedEvent(OperativeId operativeId)
        {
            OperativeId = operativeId;
        }

        public OperativeId OperativeId { get; }
    }
}