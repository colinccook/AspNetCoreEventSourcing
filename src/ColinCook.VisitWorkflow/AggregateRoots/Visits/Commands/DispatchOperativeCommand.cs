using System;
using ColinCook.VisitWorkflow.Identities;
using EventFlow.Aggregates.ExecutionResults;
using EventFlow.Commands;

namespace ColinCook.VisitWorkflow.AggregateRoots.Visits.Commands
{
    /// Command for update magic number
    public class DispatchOperativeCommand :
        Command<VisitAggregate, VisitId, IExecutionResult>
    {
        public DispatchOperativeCommand(
            VisitId aggregateId,
            OperativeId operativeId,
            DateTime estimatedArrival)
            : base(aggregateId)
        {
            OperativeId = operativeId;
            EstimatedArrival = estimatedArrival;
        }

        public OperativeId OperativeId { get; }
        public DateTime EstimatedArrival { get; }
    }
}