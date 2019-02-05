using ColinCook.VisitWorkflow.Identities;
using EventFlow.Aggregates.ExecutionResults;
using EventFlow.Commands;

namespace ColinCook.VisitWorkflow.AggregateRoots.Operatives.Commands
{
    public class OperativeHiredCommand :
        Command<OperativeAggregate, OperativeId, IExecutionResult>
    {
        public OperativeHiredCommand(OperativeId operativeId, string forename, string surname) : base(operativeId)
        {
            OperativeId = operativeId;
            Forename = forename;
            Surname = surname;
        }

        public OperativeId OperativeId { get; }
        public string Forename { get; }
        public string Surname { get; }
    }
}
