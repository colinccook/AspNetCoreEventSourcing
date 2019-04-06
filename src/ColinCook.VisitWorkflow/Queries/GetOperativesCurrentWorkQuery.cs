using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ColinCCook.AspNetCoreEventSourcing.EventFlow.AggregateRoots.Operatives.ReadModels;
using ColinCCook.AspNetCoreEventSourcing.EventFlow.AggregateRoots.Sites.ReadModels;
using ColinCCook.AspNetCoreEventSourcing.EventFlow.AggregateRoots.Works.ReadModels;
using ColinCCook.AspNetCoreEventSourcing.EventFlow.Identities;
using EventFlow.Queries;
using EventFlow.ReadStores.InMemory;

namespace ColinCCook.AspNetCoreEventSourcing.EventFlow.Queries
{
    public class GetOperativesCurrentWorkResult
    {
        public OperativeReadModel Operative { get; set; }
        public WorkReadModel Work { get; set; }
        public IReadOnlyList<SiteReadModel> Sites { get; set; }
    }

    public class GetOperativesCurrentWorkQuery : IQuery<GetOperativesCurrentWorkResult>
    {
        public OperativeId OperativeId { get; set; }
    }

    public class
        GetOperativesCurrentWorkQueryHandler : IQueryHandler<GetOperativesCurrentWorkQuery, GetOperativesCurrentWorkResult>
    {
        private readonly IInMemoryReadStore<OperativeReadModel> _operativeReadStore;
        private readonly IInMemoryReadStore<SiteReadModel> _siteReadStore;
        private readonly IInMemoryReadStore<WorkReadModel> _workReadStore;

        public GetOperativesCurrentWorkQueryHandler(IInMemoryReadStore<WorkReadModel> workReadStore,
            IInMemoryReadStore<SiteReadModel> siteReadStore, IInMemoryReadStore<OperativeReadModel> operativeReadStore)
        {
            _workReadStore = workReadStore;
            _siteReadStore = siteReadStore;
            _operativeReadStore = operativeReadStore;
        }

        public async Task<GetOperativesCurrentWorkResult> ExecuteQueryAsync(GetOperativesCurrentWorkQuery query,
            CancellationToken cancellationToken)
        {
            var operative = await _operativeReadStore.GetAsync(query.OperativeId.ToString(), cancellationToken);

            var result = new GetOperativesCurrentWorkResult
            {
                Operative = operative.ReadModel,
                Work = await GetOperativesMostRecentUnassignedWork(query.OperativeId, cancellationToken)
            };

            if (result.Work == null) return result;

            result.Sites = await GetSitesForWork(result.Work, cancellationToken);

            return result;
        }

        private async Task<WorkReadModel> GetOperativesMostRecentUnassignedWork(OperativeId operativeId,
            CancellationToken cancellationToken)
        {
            var works = await _workReadStore.FindAsync(rm => rm.AssignedOperativeId == operativeId, cancellationToken)
                .ConfigureAwait(false);

            return works.FirstOrDefault(w => !w.IsComplete);
        }

        private async Task<IReadOnlyList<SiteReadModel>> GetSitesForWork(WorkReadModel work,
            CancellationToken cancellationToken)
        {
            var result = new List<SiteReadModel>();

            foreach (var site in work.Sites)
            {
                var siteReadModel = await _siteReadStore.GetAsync(site.ToString(), cancellationToken);
                result.Add(siteReadModel.ReadModel);
            }

            return result;
        }
    }
}