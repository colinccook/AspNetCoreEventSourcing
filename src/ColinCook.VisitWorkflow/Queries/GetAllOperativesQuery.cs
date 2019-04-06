using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ColinCCook.AspNetCoreEventSourcing.EventFlow.AggregateRoots.Operatives.ReadModels;
using EventFlow.Queries;
using EventFlow.ReadStores.InMemory;

namespace ColinCCook.AspNetCoreEventSourcing.EventFlow.Queries
{
    public class GetAllOperativesQuery : IQuery<IReadOnlyList<OperativeReadModel>>
    {
    }

    public class GetAllOperativesQueryHandler : IQueryHandler<GetAllOperativesQuery, IReadOnlyList<OperativeReadModel>>
    {
        private readonly IInMemoryReadStore<OperativeReadModel> _readStore;

        public GetAllOperativesQueryHandler(IInMemoryReadStore<OperativeReadModel> readStore)
        {
            _readStore = readStore;
        }

        public async Task<IReadOnlyList<OperativeReadModel>> ExecuteQueryAsync(GetAllOperativesQuery query,
            CancellationToken cancellationToken)
        {
            var readModels = await _readStore.FindAsync(rm => true, cancellationToken).ConfigureAwait(false);
            return readModels.ToList();
        }
    }
}