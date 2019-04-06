using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ColinCCook.AspNetCoreEventSourcing.EventFlow.AggregateRoots.Sites.ReadModels;
using EventFlow.Queries;
using EventFlow.ReadStores.InMemory;

namespace ColinCCook.AspNetCoreEventSourcing.EventFlow.Queries
{
    public class AllSitesQuery : IQuery<IReadOnlyList<SiteReadModel>>
    {
    }

    public class AllSitesQueryHandler : IQueryHandler<AllSitesQuery, IReadOnlyList<SiteReadModel>>
    {
        private readonly IInMemoryReadStore<SiteReadModel> _readStore;

        public AllSitesQueryHandler(IInMemoryReadStore<SiteReadModel> readStore)
        {
            _readStore = readStore;
        }

        public async Task<IReadOnlyList<SiteReadModel>> ExecuteQueryAsync(AllSitesQuery query,
            CancellationToken cancellationToken)
        {
            var readModels = await _readStore.FindAsync(rm => true, cancellationToken).ConfigureAwait(false);
            return readModels.ToList();
        }
    }
}