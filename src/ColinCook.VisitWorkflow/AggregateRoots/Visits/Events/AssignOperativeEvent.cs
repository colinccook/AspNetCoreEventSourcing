using ColinCook.VisitWorkflow.Identities;
using EventFlow.Aggregates;
using EventFlow.EventStores;

namespace ColinCook.VisitWorkflow.AggregateRoots.Visits.Events
{
    /// A basic event containing some information
    [EventVersion(nameof(AssignOperativeEvent), 1)]
    public class AssignOperativeEvent :
        AggregateEvent<VisitAggregate, VisitId>
    {
        public AssignOperativeEvent(OperativeId operativeId)
        {
            OperativeId = operativeId;
        }

        public OperativeId OperativeId { get; }
    }
}