using EventFlow.Aggregates;
using EventFlow.ReadStores;

namespace ColinCook.Documentation.GettingStarted
{
    /// Read model for our aggregate
    public class ExampleReadModel :
        IReadModel,
        IAmReadModelFor<ExampleAggregate, ExampleId, ExampleEvent>
    {
        public int MagicNumber { get; private set; }

        public void Apply(
            IReadModelContext context,
            IDomainEvent<ExampleAggregate, ExampleId, ExampleEvent> domainEvent)
        {
            MagicNumber = domainEvent.AggregateEvent.MagicNumber;
        }
    }
}