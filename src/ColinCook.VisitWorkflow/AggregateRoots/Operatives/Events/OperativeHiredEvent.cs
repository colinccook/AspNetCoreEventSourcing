using ColinCCook.AspNetCoreEventSourcing.EventFlow.Identities;
using EventFlow.Aggregates;
using EventFlow.EventStores;

namespace ColinCCook.AspNetCoreEventSourcing.EventFlow.AggregateRoots.Operatives.Events
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