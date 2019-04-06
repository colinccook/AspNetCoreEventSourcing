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
    public class GetLatestWorkToAssignResult
    {
        public WorkReadModel Work { get; set; }
        public IList<SiteReadModel> Sites { get; set; }
        public IReadOnlyList<OperativeReadModel> Operatives { get; set; }
        public IDictionary<OperativeId, IList<WorkReadModel>> OperativesWork { get; set; }
        public IDictionary<SiteId, SiteReadModel> OperativeWorkSites { get; set; }
    }

    public class GetLatestWorkToAssignQuery : IQuery<GetLatestWorkToAssignResult>
    {
    }

    public class GetLatestWorkAssignQueryHandler : IQueryHandler<GetLatestWorkToAssignQuery, GetLatestWorkToAssignResult>
    {
        private readonly IInMemoryReadStore<OperativeReadModel> _operativeReadStore;
        private readonly IInMemoryReadStore<SiteReadModel> _siteReadStore;
        private readonly IInMemoryReadStore<WorkReadModel> _workReadStore;

        public GetLatestWorkAssignQueryHandler(IInMemoryReadStore<WorkReadModel> workReadStore,
            IInMemoryReadStore<SiteReadModel> siteReadStore, IInMemoryReadStore<OperativeReadModel> operativeReadStore)
        {
            _workReadStore = workReadStore;
            _siteReadStore = siteReadStore;
            _operativeReadStore = operativeReadStore;
        }

        public async Task<GetLatestWorkToAssignResult> ExecuteQueryAsync(GetLatestWorkToAssignQuery query,
            CancellationToken cancellationToken)
        {
            var result = new GetLatestWorkToAssignResult
            {
                Work = await GetMostRecentUnassignedWork(cancellationToken),
                Sites = new List<SiteReadModel>(),
                Operatives = await GetAllOperatives(cancellationToken),
                OperativesWork = new Dictionary<OperativeId, IList<WorkReadModel>>(),
                OperativeWorkSites = new Dictionary<SiteId, SiteReadModel>()
            };

            if (result.Work == null) return null;

            foreach (var site in result.Work.Sites)
            {
                var envelope = await _siteReadStore.GetAsync(site.ToString(), cancellationToken);
                result.Sites.Add(envelope.ReadModel);
            }

            await GetUpcomingWorkForOperatives(cancellationToken, result);

            await GetUpcomingWorkSites(cancellationToken, result);

            return result;
        }

        private async Task GetUpcomingWorkSites(CancellationToken cancellationToken, GetLatestWorkToAssignResult result)
        {
            foreach (var site in result.OperativesWork.Values.SelectMany(w => w.SelectMany(x => x.Sites)))
                if (!result.OperativeWorkSites.ContainsKey(site))
                {
                    var siteReadModel = await _siteReadStore.GetAsync(site.ToString(), cancellationToken);
                    result.OperativeWorkSites.Add(site, siteReadModel.ReadModel);
                }
        }

        private async Task GetUpcomingWorkForOperatives(CancellationToken cancellationToken,
            GetLatestWorkToAssignResult result)
        {
            foreach (var operative in result.Operatives)
            {
                var works = await _workReadStore.FindAsync(
                    rm => rm.AssignedOperativeId == operative.OperativeId && !rm.IsComplete, cancellationToken);
                result.OperativesWork.Add(operative.OperativeId, works.ToList());
            }
        }

        private async Task<WorkReadModel> GetMostRecentUnassignedWork(CancellationToken cancellationToken)
        {
            var works = await _workReadStore.FindAsync(rm => true, cancellationToken).ConfigureAwait(false);

            return works.FirstOrDefault(w => w.AssignedOperativeId == null);
        }

        private async Task<IReadOnlyList<OperativeReadModel>> GetAllOperatives(CancellationToken cancellationToken)
        {
            var operatives = await _operativeReadStore.FindAsync(rm => true, cancellationToken).ConfigureAwait(false);

            return operatives.ToList();
        }
    }
}