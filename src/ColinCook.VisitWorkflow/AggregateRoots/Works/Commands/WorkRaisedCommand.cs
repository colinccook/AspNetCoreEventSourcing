using System;
using System.Collections.Generic;
using System.Text;
using ColinCook.VisitWorkflow.Identities;
using EventFlow.Aggregates.ExecutionResults;
using EventFlow.Commands;

namespace ColinCook.VisitWorkflow.AggregateRoots.Works.Commands
{
    public class WorkRaisedCommand :
        Command<WorkAggregate, WorkId, IExecutionResult>
    {
        public WorkRaisedCommand(WorkId workId, IEnumerable<SiteId> sites, string title, string description ) : base(workId)
        {
            WorkId = workId;
            Sites = sites;
            Title = title;
            Description = description;
        }

        public WorkId WorkId { get; }
        public IEnumerable<SiteId> Sites { get; }
        public string Title { get; }
        public string Description { get; }
    }
}
