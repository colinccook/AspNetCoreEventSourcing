using ColinCCook.AspNetCoreEventSourcing.EventFlow.AggregateRoots.Operatives.Events;
using ColinCCook.AspNetCoreEventSourcing.EventFlow.Identities;
using EventFlow.Aggregates;
using EventFlow.ReadStores;

namespace ColinCCook.AspNetCoreEventSourcing.EventFlow.AggregateRoots.Operatives.ReadModels
{
    public class OperativeReadModel : IReadModel,
        IAmReadModelFor<OperativeAggregate, OperativeId, OperativeHiredEvent>
    {
        public OperativeId OperativeId { get; private set; }
        public string Forename { get; private set; }
        public string Surname { get; private set; }

        public void Apply(IReadModelContext context,
            IDomainEvent<OperativeAggregate, OperativeId, OperativeHiredEvent> domainEvent)
        {
            OperativeId = domainEvent.AggregateIdentity;
            Forename = domainEvent.AggregateEvent.Forename;
            Surname = domainEvent.AggregateEvent.Surname;
        }
    }
}