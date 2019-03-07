using EventFlow.Core;

namespace EventFlow.Documentation.Topics.Commands
{
    public class UserId : Identity<UserId>, ISourceId
    {
        public UserId(string value) : base(value)
        {
        }
    }
}