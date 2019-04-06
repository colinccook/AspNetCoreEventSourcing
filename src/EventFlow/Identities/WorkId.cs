using EventFlow.Core;
using EventFlow.ValueObjects;
using Newtonsoft.Json;

namespace ColinCCook.AspNetCoreEventSourcing.EventFlow.Identities
{
    /// Represents the aggregate identity (ID)
    [JsonConverter(typeof(SingleValueObjectConverter))]
    public class WorkId :
        Identity<WorkId>
    {
        public WorkId(string value) : base(value)
        {
        }
    }
}