using ColinCook.VisitWorkflow.Identities;

namespace ColinCook.VisitWorkflow.AggregateRoots.Sites.Models
{
    public class SiteModel
    {
        public SiteId SiteId { get; set; }
        public string AddressLine1 { get; set; }
        public string Town { get; set; }
        public string PostCode { get; set;  }
        public string TelephoneNumber { get; set; }
        // IsChecked is leaking a UI concern in the domain
        public bool IsChecked { get; set; }
    }
}
