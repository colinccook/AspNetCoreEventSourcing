using EventFlow.ValueObjects;

namespace EventFlow.Documentation.Topics.Commands
{
    public class Password : SingleValueObject<string>
    {
        public Password(string value) : base(value)
        {
        }
    }
}