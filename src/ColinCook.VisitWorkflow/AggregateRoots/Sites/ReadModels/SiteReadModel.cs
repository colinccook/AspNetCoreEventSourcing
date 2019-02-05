using ColinCook.VisitWorkflow.AggregateRoots.Operatives;
using ColinCook.VisitWorkflow.AggregateRoots.Operatives.Events;
using ColinCook.VisitWorkflow.AggregateRoots.Operatives.Models;
using ColinCook.VisitWorkflow.AggregateRoots.Sites.Events;
using ColinCook.VisitWorkflow.AggregateRoots.Sites.Models;
using ColinCook.VisitWorkflow.Identities;
using EventFlow.Aggregates;
using EventFlow.ReadStores;

namespace ColinCook.VisitWorkflow.AggregateRoots.Sites.ReadModels
{
    public class SiteReadModel : IReadModel,
        IAmReadModelFor<SiteAggregate, SiteId, SiteAcquiredEvent>
    {
        public SiteModel SiteModel { get; private set; }

        public void Apply(IReadModelContext context, IDomainEvent<SiteAggregate, SiteId, SiteAcquiredEvent> domainEvent)
        {
            SiteModel = new SiteModel
            {
                SiteId = domainEvent.AggregateIdentity,
                AddressLine1 = domainEvent.AggregateEvent.AddressLine1,
                Town = domainEvent.AggregateEvent.Town,
                PostCode = domainEvent.AggregateEvent.PostCode,
                TelephoneNumber = domainEvent.AggregateEvent.TelephoneNumber,
            };
        }
    }
}
