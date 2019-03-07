using EventFlow.Core;

namespace ColinCook.Documentation.GettingStarted
{
    /// Represents the aggregate identity (ID)
    public class ExampleId :
        Identity<ExampleId>
    {
        public ExampleId(string value) : base(value)
        {
        }
    }
}