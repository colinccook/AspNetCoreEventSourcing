using ColinCook.VisitWorkflow.Identities;
using EventFlow.Aggregates.ExecutionResults;
using EventFlow.Commands;

namespace ColinCook.VisitWorkflow.AggregateRoots.Sites.Commands
{
    public class SiteAcquiredCommand :
        Command<SiteAggregate, SiteId, IExecutionResult>
    {
        public SiteAcquiredCommand(SiteId siteId, string addressLine1, string town, string postCode, string telephoneNumber) : base(siteId)
        {
            SiteId = siteId;
            AddressLine1 = addressLine1;
            Town = town;
            PostCode = postCode;
            TelephoneNumber = telephoneNumber;
        }

        public SiteId SiteId { get; }
        public string AddressLine1 { get; }
        public string Town { get; }
        public string PostCode { get; }
        public string TelephoneNumber { get; }
    }
}
