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

namespace ColinCook.VisitWorkflow.AggregateRoots.Works.Queries
{
    public class AssignWorkQueryResult
    {
        public WorkReadModel Work { get; set; }
        public IList<SiteReadModel> Sites { get; set; }
        public IReadOnlyList<OperativeReadModel> Operatives { get; set; }
        public IDictionary<OperativeId, IList<WorkReadModel>> OperativesWork { get; set; }
        public IDictionary<SiteId, SiteReadModel> OperativeWorkSites { get; set; }
    }

    public class AssignWorkQuery : IQuery<AssignWorkQueryResult>
    {
    }

    public class AssignWorkQueryHandler : IQueryHandler<AssignWorkQuery, AssignWorkQueryResult>
    {
        private readonly IInMemoryReadStore<OperativeReadModel> _operativeReadStore;
        private readonly IInMemoryReadStore<SiteReadModel> _siteReadStore;
        private readonly IInMemoryReadStore<WorkReadModel> _workReadStore;

        public AssignWorkQueryHandler(IInMemoryReadStore<WorkReadModel> workReadStore,
            IInMemoryReadStore<SiteReadModel> siteReadStore, IInMemoryReadStore<OperativeReadModel> operativeReadStore)
        {
            _workReadStore = workReadStore;
            _siteReadStore = siteReadStore;
            _operativeReadStore = operativeReadStore;
        }

        public async Task<AssignWorkQueryResult> ExecuteQueryAsync(AssignWorkQuery query,
            CancellationToken cancellationToken)
        {
            AssignWorkQueryResult result = new AssignWorkQueryResult
            {
                Work = await GetMostRecentUnassignedWork(cancellationToken),
                Sites = new List<SiteReadModel>(),
                Operatives = await GetAllOperatives(cancellationToken),
                OperativesWork = new Dictionary<OperativeId, IList<WorkReadModel>>(),
                OperativeWorkSites = new Dictionary<SiteId, SiteReadModel>()
            };

            if (result.Work == null)
            {
                return null;
            }

            foreach (SiteId site in result.Work.Sites)
            {
                EventFlow.ReadStores.ReadModelEnvelope<SiteReadModel> envelope = await _siteReadStore.GetAsync(site.ToString(), cancellationToken);
                result.Sites.Add(envelope.ReadModel);
            }

            await GetUpcomingWorkForOperatives(cancellationToken, result);

            await GetUpcomingWorkSites(cancellationToken, result);

            return result;
        }

        private async Task GetUpcomingWorkSites(CancellationToken cancellationToken, AssignWorkQueryResult result)
        {
            foreach (SiteId site in result.OperativesWork.Values.SelectMany(w => w.SelectMany(x => x.Sites)))
            {
                if (!result.OperativeWorkSites.ContainsKey(site))
                {
                    EventFlow.ReadStores.ReadModelEnvelope<SiteReadModel> siteReadModel = await _siteReadStore.GetAsync(site.ToString(), cancellationToken);
                    result.OperativeWorkSites.Add(site, siteReadModel.ReadModel);
                }
            }
        }

        private async Task GetUpcomingWorkForOperatives(CancellationToken cancellationToken,
            AssignWorkQueryResult result)
        {
            foreach (OperativeReadModel operative in result.Operatives)
            {
                IReadOnlyCollection<WorkReadModel> works = await _workReadStore.FindAsync(
                    rm => rm.AssignedOperativeId == operative.OperativeId && !rm.IsComplete, cancellationToken);
                result.OperativesWork.Add(operative.OperativeId, works.ToList());
            }
        }

        private async Task<WorkReadModel> GetMostRecentUnassignedWork(CancellationToken cancellationToken)
        {
            IReadOnlyCollection<WorkReadModel> works = await _workReadStore.FindAsync(rm => true, cancellationToken).ConfigureAwait(false);

            return works.FirstOrDefault(w => w.AssignedOperativeId == null);
        }

        private async Task<IReadOnlyList<OperativeReadModel>> GetAllOperatives(CancellationToken cancellationToken)
        {
            IReadOnlyCollection<OperativeReadModel> operatives = await _operativeReadStore.FindAsync(rm => true, cancellationToken).ConfigureAwait(false);

            return operatives.ToList();
        }
    }
}