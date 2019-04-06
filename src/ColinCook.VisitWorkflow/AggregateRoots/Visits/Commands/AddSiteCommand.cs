using ColinCCook.AspNetCoreEventSourcing.EventFlow.Identities;
using EventFlow.Aggregates.ExecutionResults;
using EventFlow.Commands;

namespace ColinCCook.AspNetCoreEventSourcing.EventFlow.AggregateRoots.Visits.Commands
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