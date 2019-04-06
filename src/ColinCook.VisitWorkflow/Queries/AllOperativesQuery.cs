using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ColinCook.VisitWorkflow.AggregateRoots.Operatives.ReadModels;
using EventFlow.Queries;
using EventFlow.ReadStores.InMemory;

namespace ColinCook.VisitWorkflow.Queries
{
    public class AllOperativesQuery : IQuery<IReadOnlyList<OperativeReadModel>>
    {
    }

    public class AllOperativesQueryHandler : IQueryHandler<AllOperativesQuery, IReadOnlyList<OperativeReadModel>>
    {
        private readonly IInMemoryReadStore<OperativeReadModel> _readStore;

        public AllOperativesQueryHandler(IInMemoryReadStore<OperativeReadModel> readStore)
        {
            _readStore = readStore;
        }

        public async Task<IReadOnlyList<OperativeReadModel>> ExecuteQueryAsync(AllOperativesQuery query,
            CancellationToken cancellationToken)
        {
            var readModels = await _readStore.FindAsync(rm => true, cancellationToken).ConfigureAwait(false);
            return readModels.ToList();
        }
    }
}