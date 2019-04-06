using System;
using ColinCCook.AspNetCoreEventSourcing.EventFlow.Identities;
using EventFlow.Aggregates;
using EventFlow.EventStores;

namespace ColinCCook.AspNetCoreEventSourcing.EventFlow.AggregateRoots.Visits.Events
{
    /// A basic event containing some information
    [EventVersion(nameof(DispatchOperativeEvent), 1)]
    public class DispatchOperativeEvent :
        AggregateEvent<VisitAggregate, VisitId>
    {
        public DispatchOperativeEvent(OperativeId operativeId, DateTime estimatedArrival)
        {
            OperativeId = operativeId;
            EstimatedArrival = estimatedArrival;
        }

        public OperativeId OperativeId { get; }
        public DateTime EstimatedArrival { get; }
    }
}