using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ColinCook.VisitWorkflow.AggregateRoots.Operatives.Models;
using ColinCook.VisitWorkflow.AggregateRoots.Operatives.Queries;
using ColinCook.VisitWorkflow.AggregateRoots.Operatives.ReadModels;
using EventFlow.Queries;
using EventFlow.ReadStores.InMemory;

namespace ColinCook.VisitWorkflow.AggregateRoots.Operatives.QueryHandlers
{
    public class AllOperativesQueryHandler : IQueryHandler<AllOperativesQuery, IReadOnlyCollection<OperativeModel>>
    {
        private readonly IInMemoryReadStore<OperativeReadModel> _readStore;

        public AllOperativesQueryHandler(IInMemoryReadStore<OperativeReadModel> readStore)
        {
            _readStore = readStore;
        }

        public async Task<IReadOnlyCollection<OperativeModel>> ExecuteQueryAsync(AllOperativesQuery query, CancellationToken cancellationToken)
        {
            var readModels = await _readStore.FindAsync(rm => true, cancellationToken).ConfigureAwait(false);
            return readModels.Select(rm => rm.OperativeModel).ToList();
        }
    }
}
