using ColinCCook.AspNetCoreEventSourcing.EventFlow.Identities;
using EventFlow.Aggregates.ExecutionResults;
using EventFlow.Commands;

namespace ColinCCook.AspNetCoreEventSourcing.EventFlow.AggregateRoots.Visits.Commands
{
    /// Command for update magic number
    public class AssignOperativeCommand :
        Command<VisitAggregate, VisitId, IExecutionResult>
    {
        public AssignOperativeCommand(
            VisitId aggregateId,
            OperativeId operativeId)
            : base(aggregateId)
        {
            OperativeId = operativeId;
        }

        public OperativeId OperativeId { get; }
    }
}