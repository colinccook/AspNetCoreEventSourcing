using ColinCook.VisitWorkflow.AggregateRoots.Operatives.ReadModels;
using ColinCook.VisitWorkflow.AggregateRoots.Sites.ReadModels;
using ColinCook.VisitWorkflow.AggregateRoots.Works.ReadModels;
using ColinCook.VisitWorkflow.Identities;
using EventFlow.Queries;
using EventFlow.ReadStores.InMemory;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ColinCook.VisitWorkflow.AggregateRoots.Operatives.Queries
{
    public class GetOperativeAndWorkQueryResult
    {
        public OperativeReadModel Operative { get; set; }
        public WorkReadModel Work { get; set; }
        public IReadOnlyList<SiteReadModel> Sites { get; set; }
    }

    public class GetOperativeAndWorkQuery : IQuery<GetOperativeAndWorkQueryResult>
    {
        public OperativeId OperativeId { get; set; }
    }

    public class
        GetOperativeAndWorkQueryHandler : IQueryHandler<GetOperativeAndWorkQuery, GetOperativeAndWorkQueryResult>
    {
        private readonly IInMemoryReadStore<OperativeReadModel> _operativeReadStore;
        private readonly IInMemoryReadStore<SiteReadModel> _siteReadStore;
        private readonly IInMemoryReadStore<WorkReadModel> _workReadStore;

        public GetOperativeAndWorkQueryHandler(IInMemoryReadStore<WorkReadModel> workReadStore,
            IInMemoryReadStore<SiteReadModel> siteReadStore, IInMemoryReadStore<OperativeReadModel> operativeReadStore)
        {
            _workReadStore = workReadStore;
            _siteReadStore = siteReadStore;
            _operativeReadStore = operativeReadStore;
        }

        public async Task<GetOperativeAndWorkQueryResult> ExecuteQueryAsync(GetOperativeAndWorkQuery query,
            CancellationToken cancellationToken)
        {
            EventFlow.ReadStores.ReadModelEnvelope<OperativeReadModel> operative = await _operativeReadStore.GetAsync(query.OperativeId.ToString(), cancellationToken);

            GetOperativeAndWorkQueryResult result = new GetOperativeAndWorkQueryResult
            {
                Operative = operative.ReadModel,
                Work = await GetOperativesMostRecentUnassignedWork(query.OperativeId, cancellationToken)
            };

            if (result.Work == null)
            {
                return result;
            }

            result.Sites = await GetSitesForWork(result.Work, cancellationToken);

            return result;
        }

        private async Task<WorkReadModel> GetOperativesMostRecentUnassignedWork(OperativeId operativeId,
            CancellationToken cancellationToken)
        {
            IReadOnlyCollection<WorkReadModel> works = await _workReadStore.FindAsync(rm => rm.AssignedOperativeId == operativeId, cancellationToken)
                .ConfigureAwait(false);

            return works.FirstOrDefault(w => !w.IsComplete);
        }

        private async Task<IReadOnlyList<SiteReadModel>> GetSitesForWork(WorkReadModel work,
            CancellationToken cancellationToken)
        {
            List<SiteReadModel> result = new List<SiteReadModel>();

            foreach (SiteId site in work.Sites)
            {
                EventFlow.ReadStores.ReadModelEnvelope<SiteReadModel> siteReadModel = await _siteReadStore.GetAsync(site.ToString(), cancellationToken);
                result.Add(siteReadModel.ReadModel);
            }

            return result;
        }
    }
}