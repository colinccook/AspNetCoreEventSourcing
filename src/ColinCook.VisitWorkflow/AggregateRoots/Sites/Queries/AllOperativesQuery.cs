using System.Collections.Generic;
using ColinCook.VisitWorkflow.AggregateRoots.Sites.Models;
using EventFlow.Queries;

namespace ColinCook.VisitWorkflow.AggregateRoots.Sites.Queries
{
    public class AllSitesQuery : IQuery<IReadOnlyList<SiteModel>>
    {
    }
}
