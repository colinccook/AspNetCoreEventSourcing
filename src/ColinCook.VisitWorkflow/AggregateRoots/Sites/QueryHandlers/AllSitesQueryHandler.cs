using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ColinCook.VisitWorkflow.AggregateRoots.Operatives.Models;
using ColinCook.VisitWorkflow.AggregateRoots.Operatives.Queries;
using ColinCook.VisitWorkflow.AggregateRoots.Operatives.ReadModels;
using ColinCook.VisitWorkflow.AggregateRoots.Sites.Models;
using ColinCook.VisitWorkflow.AggregateRoots.Sites.Queries;
using ColinCook.VisitWorkflow.AggregateRoots.Sites.ReadModels;
using EventFlow.Queries;
using EventFlow.ReadStores.InMemory;

namespace ColinCook.VisitWorkflow.AggregateRoots.Sites.QueryHandlers
{
    public class AllSitesQueryHandler : IQueryHandler<AllSitesQuery, IReadOnlyList<SiteModel>>
    {
        private readonly IInMemoryReadStore<SiteReadModel> _readStore;

        public AllSitesQueryHandler(IInMemoryReadStore<SiteReadModel> readStore)
        {
            _readStore = readStore;
        }

        public async Task<IReadOnlyList<SiteModel>> ExecuteQueryAsync(AllSitesQuery query, CancellationToken cancellationToken)
        {
            var readModels = await _readStore.FindAsync(rm => true, cancellationToken).ConfigureAwait(false);
            return readModels.Select(rm => rm.SiteModel).ToList();
        }
    }
}
