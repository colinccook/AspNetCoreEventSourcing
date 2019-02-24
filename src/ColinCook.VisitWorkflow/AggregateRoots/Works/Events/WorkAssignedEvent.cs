using System;
using System.Collections.Generic;
using System.Text;
using ColinCook.VisitWorkflow.Identities;
using EventFlow.Aggregates;
using EventFlow.EventStores;

namespace ColinCook.VisitWorkflow.AggregateRoots.Works.Events
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
