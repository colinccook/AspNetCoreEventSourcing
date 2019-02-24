using ColinCook.VisitWorkflow.AggregateRoots.Works.Events;
using ColinCook.VisitWorkflow.Identities;
using EventFlow.Aggregates;
using EventFlow.Aggregates.ExecutionResults;
using System.Collections.Generic;
using System.Linq;

namespace ColinCook.VisitWorkflow.AggregateRoots.Works
{
    public class WorkAggregate :
        AggregateRoot<WorkAggregate, WorkId>,
        IEmit<WorkRaisedEvent>,
        IEmit<WorkAssignedEvent>
    {
        private string _title;
        private string _description;
        private IEnumerable<SiteId> _sites;
        private OperativeId _operative;

        public WorkAggregate(WorkId id) : base(id)
        {
        }

        public void Apply(WorkRaisedEvent aggregateEvent)
        {
            _title = aggregateEvent.Title;
            _description = aggregateEvent.Description;
            _sites = aggregateEvent.Sites;
        }

        internal IExecutionResult Raise(string title, string description, IEnumerable<SiteId> sites)
        {
            if (string.IsNullOrWhiteSpace(title))
            {
                return ExecutionResult.Failed("Work must have a title");
            }

            if (string.IsNullOrWhiteSpace(description))
            {
                return ExecutionResult.Failed("Work must have a description");
            }

            if (!sites.Any())
            {
                return ExecutionResult.Failed("Work must have at least one Site added");
            }

            Emit(new WorkRaisedEvent(title, description, sites));

            return ExecutionResult.Success();
        }

        internal IExecutionResult Assign(OperativeId operativeId)
        {
            if (_operative != null)
            {
                return ExecutionResult.Failed("An operative is already assigned to this work");
            }

            Emit(new WorkAssignedEvent(operativeId));

            return ExecutionResult.Success();
        }

        public void Apply(WorkAssignedEvent aggregateEvent)
        {
            _operative = aggregateEvent.OperativeId;
        }
    }
}