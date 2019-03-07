using ColinCook.VisitWorkflow.Identities;
using EventFlow.Aggregates.ExecutionResults;
using EventFlow.Commands;

namespace ColinCook.VisitWorkflow.AggregateRoots.Visits.Commands
{
    /// Command for update magic number
    public class AddSiteCommand :
        Command<VisitAggregate, VisitId, IExecutionResult>
    {
        public AddSiteCommand(
            VisitId aggregateId,
            SiteId siteId)
            : base(aggregateId)
        {
            SiteId = siteId;
        }

        public SiteId SiteId { get; }
    }
}