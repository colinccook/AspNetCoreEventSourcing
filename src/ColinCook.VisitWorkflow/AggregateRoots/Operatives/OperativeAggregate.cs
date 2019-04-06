using ColinCCook.AspNetCoreEventSourcing.EventFlow.AggregateRoots.Operatives.Events;
using ColinCCook.AspNetCoreEventSourcing.EventFlow.Identities;
using EventFlow.Aggregates;
using EventFlow.Aggregates.ExecutionResults;

namespace ColinCCook.AspNetCoreEventSourcing.EventFlow.AggregateRoots.Operatives
{
    /// The aggregate root
    public class OperativeAggregate :
        AggregateRoot<OperativeAggregate, OperativeId>,
        IEmit<OperativeHiredEvent>
    {
        private string _forename;
        private string _surname;

        public OperativeAggregate(OperativeId id) : base(id)
        {
        }

        public void Apply(OperativeHiredEvent aggregateEvent)
        {
            _forename = aggregateEvent.Forename;
            _surname = aggregateEvent.Surname;
        }

        public IExecutionResult Hire(string forename, string surname)
        {
            if (string.IsNullOrWhiteSpace(forename)) return ExecutionResult.Failed("Operative must have a forename");

            if (string.IsNullOrWhiteSpace(surname)) return ExecutionResult.Failed("Operative must have a surname");

            Emit(new OperativeHiredEvent(forename, surname));

            return ExecutionResult.Success();
        }
    }
}