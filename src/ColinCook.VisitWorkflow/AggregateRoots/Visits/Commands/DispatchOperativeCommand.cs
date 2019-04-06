using System;
using ColinCCook.AspNetCoreEventSourcing.EventFlow.Identities;
using EventFlow.Aggregates.ExecutionResults;
using EventFlow.Commands;

namespace ColinCCook.AspNetCoreEventSourcing.EventFlow.AggregateRoots.Visits.Commands
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