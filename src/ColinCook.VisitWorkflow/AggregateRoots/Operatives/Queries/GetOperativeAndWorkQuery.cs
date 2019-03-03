using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ColinCook.VisitWorkflow.AggregateRoots.Operatives.ReadModels;
using ColinCook.VisitWorkflow.AggregateRoots.Sites.ReadModels;
using ColinCook.VisitWorkflow.AggregateRoots.Works.ReadModels;
using ColinCook.VisitWorkflow.Identities;
using EventFlow.Aggregates;
using EventFlow.Queries;
using EventFlow.ReadStores.InMemory;

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

    public class GetOperativeAndWorkQueryHandler : IQueryHandler<GetOperativeAndWorkQuery, GetOperativeAndWorkQueryResult>
    {
        private readonly IInMemoryReadStore<WorkReadModel> _workReadStore;
        private readonly IInMemoryReadStore<SiteReadModel> _siteReadStore;
        private readonly IInMemoryReadStore<OperativeReadModel> _operativeReadStore;


        public GetOperativeAndWorkQueryHandler(IInMemoryReadStore<WorkReadModel> workReadStore, IInMemoryReadStore<SiteReadModel> siteReadStore, IInMemoryReadStore<OperativeReadModel> operativeReadStore)
        {
            _workReadStore = workReadStore;
            _siteReadStore = siteReadStore;
            _operativeReadStore = operativeReadStore;
        }

        public async Task<GetOperativeAndWorkQueryResult> ExecuteQueryAsync(GetOperativeAndWorkQuery query, CancellationToken cancellationToken)
        {
            var operative = await _operativeReadStore.GetAsync(query.OperativeId.ToString(), cancellationToken);

            var result = new GetOperativeAndWorkQueryResult
            {
                Operative = operative.ReadModel,
                Work = await GetOperativesMostRecentUnassignedWork(query.OperativeId, cancellationToken)
            };

            if (result.Work == null)
                return result;

            result.Sites = await GetSitesForWork(result.Work, cancellationToken); 
            
            return result;
        }

        private async Task<WorkReadModel> GetOperativesMostRecentUnassignedWork(OperativeId operativeId, CancellationToken cancellationToken)
        {
            var works = await _workReadStore.FindAsync(rm => rm.AssignedOperativeId == operativeId, cancellationToken).ConfigureAwait(false);

            return works.FirstOrDefault(w => !w.IsComplete);
        }

        private async Task<IReadOnlyList<SiteReadModel>> GetSitesForWork(WorkReadModel work, CancellationToken cancellationToken)
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
