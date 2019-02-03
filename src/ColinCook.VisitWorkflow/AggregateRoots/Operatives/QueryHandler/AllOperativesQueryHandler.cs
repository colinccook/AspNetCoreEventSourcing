using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ColinCook.VisitWorkflow.AggregateRoots.Operatives.Queries;
using ColinCook.VisitWorkflow.AggregateRoots.Operatives.ReadModels;
using EventFlow.Queries;
using EventFlow.ReadStores.InMemory;

namespace ColinCook.VisitWorkflow.AggregateRoots.Operatives.QueryHandler
{
    public class AllOperativesQueryHandler : IQueryHandler<AllOperativesQuery, IReadOnlyCollection<string>>
    {
        private readonly IInMemoryReadStore<AllOperativesReadModel> _readStore;

        public AllOperativesQueryHandler(IInMemoryReadStore<AllOperativesReadModel> readStore)
        {
            _readStore = readStore;
        }

        public async Task<IReadOnlyCollection<string>> ExecuteQueryAsync(AllOperativesQuery query, CancellationToken cancellationToken)
        {
            var readModels = await _readStore.FindAsync(rm => true, cancellationToken).ConfigureAwait(false);
            return readModels.Select(rm => rm.Forename).ToList();
        }
    }
}
