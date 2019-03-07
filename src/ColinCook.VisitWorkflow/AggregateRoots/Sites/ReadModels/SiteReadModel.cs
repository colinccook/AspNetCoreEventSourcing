using ColinCook.VisitWorkflow.AggregateRoots.Sites.Events;
using ColinCook.VisitWorkflow.Identities;
using EventFlow.Aggregates;
using EventFlow.ReadStores;

namespace ColinCook.VisitWorkflow.AggregateRoots.Sites.ReadModels
{
    public class SiteReadModel : IReadModel,
        IAmReadModelFor<SiteAggregate, SiteId, SiteAcquiredEvent>
    {
        public SiteId SiteId { get; private set; }
        public string AddressLine1 { get; private set; }
        public string Town { get; private set; }
        public string PostCode { get; private set; }
        public string TelephoneNumber { get; private set; }

        // IsChecked is an example where a UI concern has leaked into an inner layer. However, I'm trying to keep 
        // this sample as simple as possible and I can live with this.
        public bool IsChecked { get; set; }

        public void Apply(IReadModelContext context, IDomainEvent<SiteAggregate, SiteId, SiteAcquiredEvent> domainEvent)
        {
            SiteId = domainEvent.AggregateIdentity;
            AddressLine1 = domainEvent.AggregateEvent.AddressLine1;
            Town = domainEvent.AggregateEvent.Town;
            PostCode = domainEvent.AggregateEvent.PostCode;
            TelephoneNumber = domainEvent.AggregateEvent.TelephoneNumber;
        }
    }
}