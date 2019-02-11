using ColinCook.VisitWorkflow.Identities;
using EventFlow.Aggregates;
using EventFlow.EventStores;

namespace ColinCook.VisitWorkflow.AggregateRoots.Operatives.Events
{
    [EventVersion(nameof(OperativeHiredEvent), 1)]
    public class OperativeHiredEvent : AggregateEvent<OperativeAggregate, OperativeId>
    {
        public OperativeHiredEvent(string forename, string surname)
        {
            Forename = forename;
            Surname = surname;
        }

        public string Forename { get; }
        public string Surname { get; }
    }
}