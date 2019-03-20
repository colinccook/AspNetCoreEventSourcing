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
        IEmit<WorkAssignedEvent>,
        IEmit<WorkAbandonedEvent>,
        IEmit<WorkCompletedEvent>
    {
        private string _description;
        private bool _isComplete;
        private OperativeId _operative;
        private IEnumerable<SiteId> _sites;
        private string _title;

        public WorkAggregate(WorkId id) : base(id)
        {
        }

        public void Apply(WorkAbandonedEvent aggregateEvent)
        {
            _operative = null;
        }

        public void Apply(WorkAssignedEvent aggregateEvent)
        {
            _operative = aggregateEvent.OperativeId;
        }

        public void Apply(WorkCompletedEvent aggregateEvent)
        {
            _operative = null;
            _isComplete = true;
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
            if (_isComplete)
            {
                return ExecutionResult.Failed("You cannot assign work if it's already completed");
            }

            if (_operative != null)
            {
                return ExecutionResult.Failed("An operative is already assigned to this work");
            }

            Emit(new WorkAssignedEvent(operativeId));

            return ExecutionResult.Success();
        }

        public IExecutionResult Abandon()
        {
            if (_isComplete)
            {
                return ExecutionResult.Failed("An operative cannot abandon work when it's already been completed");
            }

            if (_operative == null)
            {
                return ExecutionResult.Failed("There is no operative assigned to this work to abandon it");
            }

            Emit(new WorkAbandonedEvent());

            return ExecutionResult.Success();
        }

        public IExecutionResult Complete()
        {
            if (_isComplete)
            {
                return ExecutionResult.Failed("You cannot complete work that's already completed");
            }

            if (_operative == null)
            {
                return ExecutionResult.Failed("There must be an operative assigned to complete the work");
            }

            Emit(new WorkCompletedEvent());

            return ExecutionResult.Success();
        }
    }
}