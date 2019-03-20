using ColinCook.VisitWorkflow.AggregateRoots.Works.Events;
using ColinCook.VisitWorkflow.Identities;
using EventFlow.Aggregates;
using EventFlow.ReadStores;
using System.Collections.Generic;

namespace ColinCook.VisitWorkflow.AggregateRoots.Works.ReadModels
{
    public class WorkReadModel :
        IReadModel,
        IAmReadModelFor<WorkAggregate, WorkId, WorkRaisedEvent>,
        IAmReadModelFor<WorkAggregate, WorkId, WorkAssignedEvent>,
        IAmReadModelFor<WorkAggregate, WorkId, WorkCompletedEvent>,
        IAmReadModelFor<WorkAggregate, WorkId, WorkAbandonedEvent>
    {
        public IEnumerable<SiteId> Sites { get; set; }
        public string Description { get; set; }
        public string Title { get; set; }
        public OperativeId AssignedOperativeId { get; set; }
        public WorkId WorkId { get; set; }
        public bool IsComplete { get; set; }

        public void Apply(IReadModelContext context,
            IDomainEvent<WorkAggregate, WorkId, WorkAbandonedEvent> domainEvent)
        {
            AssignedOperativeId = null;
        }

        public void Apply(IReadModelContext context, IDomainEvent<WorkAggregate, WorkId, WorkAssignedEvent> domainEvent)
        {
            AssignedOperativeId = domainEvent.AggregateEvent.OperativeId;
        }

        public void Apply(IReadModelContext context,
            IDomainEvent<WorkAggregate, WorkId, WorkCompletedEvent> domainEvent)
        {
            IsComplete = true;
        }

        public void Apply(IReadModelContext context,
            IDomainEvent<WorkAggregate, WorkId, WorkRaisedEvent> domainEvent)
        {
            Sites = domainEvent.AggregateEvent.Sites;
            Description = domainEvent.AggregateEvent.Description;
            Title = domainEvent.AggregateEvent.Title;
            WorkId = domainEvent.AggregateIdentity;
        }
    }
}