using EventFlow.Aggregates;
using EventFlow.EventStores;

namespace EventFlow.Documentation.GettingStarted
{
    /// A basic event containing some information
    [EventVersion("example", 1)]
    public class ExampleEvent :
        AggregateEvent<ExampleAggregate, ExampleId>
    {
        public ExampleEvent(int magicNumber)
        {
            MagicNumber = magicNumber;
        }

        public int MagicNumber { get; }
    }
}