using ColinCook.VisitWorkflow.AggregateRoots.Visits.Events;
using ColinCook.VisitWorkflow.Identities;
using EventFlow.Aggregates;
using EventFlow.ReadStores;
using System.Collections.Generic;

namespace ColinCook.VisitWorkflow.AggregateRoots.Visits.ReadModels
{
    /// Read model for our aggregate
    public class VisitReadModel :
        IReadModel,
        IAmReadModelFor<VisitAggregate, VisitId, AddSiteEvent>,
        IAmReadModelFor<VisitAggregate, VisitId, AssignOperativeEvent>,
        IAmReadModelFor<VisitAggregate, VisitId, DispatchOperativeEvent>
    {
        public VisitReadModel()
        {
            Sites = new List<SiteId>();
            AssignedOperatives = new List<OperativeId>();
            DispatchedOperatives = new List<OperativeId>();
        }

        public List<SiteId> Sites { get; }
        public List<OperativeId> AssignedOperatives { get; }
        public List<OperativeId> DispatchedOperatives { get; }

        public void Apply(
            IReadModelContext context,
            IDomainEvent<VisitAggregate, VisitId, AddSiteEvent> domainEvent)
        {
            Sites.Add(domainEvent.AggregateEvent.SiteId);
        }

        public void Apply(
            IReadModelContext context,
            IDomainEvent<VisitAggregate, VisitId, AssignOperativeEvent> domainEvent)
        {
            AssignedOperatives.Add(domainEvent.AggregateEvent.OperativeId);
        }

        public void Apply(IReadModelContext context,
            IDomainEvent<VisitAggregate, VisitId, DispatchOperativeEvent> domainEvent)
        {
            DispatchedOperatives.Add(domainEvent.AggregateEvent.OperativeId);
        }
    }
}