using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ColinCCook.AspNetCoreEventSourcing.EventFlow.AggregateRoots.Sites.ReadModels;
using EventFlow.Queries;
using EventFlow.ReadStores.InMemory;

namespace ColinCCook.AspNetCoreEventSourcing.EventFlow.Queries
{
    public class GetAllSitesQuery : IQuery<IReadOnlyList<SiteReadModel>>
    {
    }

    public class GetAllSitesQueryHandler : IQueryHandler<GetAllSitesQuery, IReadOnlyList<SiteReadModel>>
    {
        private readonly IInMemoryReadStore<SiteReadModel> _readStore;

        public GetAllSitesQueryHandler(IInMemoryReadStore<SiteReadModel> readStore)
        {
            _readStore = readStore;
        }

        public async Task<IReadOnlyList<SiteReadModel>> ExecuteQueryAsync(GetAllSitesQuery query,
            CancellationToken cancellationToken)
        {
            var readModels = await _readStore.FindAsync(rm => true, cancellationToken).ConfigureAwait(false);
            return readModels.ToList();
        }
    }
}