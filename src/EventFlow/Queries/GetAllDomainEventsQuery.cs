using System.Threading;
using System.Threading.Tasks;
using ColinCCook.AspNetCoreEventSourcing.EventFlow.Identities;
using EventFlow.EventStores;
using EventFlow.Queries;

namespace ColinCCook.AspNetCoreEventSourcing.EventFlow.Queries
{
    public class GetAllDomainEventsQuery : IQuery<AllCommittedEventsPage>
    {
        public OperativeId OperativeId { get; set; }
    }

    public class GetAllDomainEventsQueryHandler : IQueryHandler<GetAllDomainEventsQuery, AllCommittedEventsPage>
    {
        private readonly IEventPersistence _eventPersistence;

        public GetAllDomainEventsQueryHandler(IEventPersistence eventPersistence)
        {
            _eventPersistence = eventPersistence;
        }

        public async Task<AllCommittedEventsPage> ExecuteQueryAsync(GetAllDomainEventsQuery query, CancellationToken cancellationToken)
        {
            var result = await _eventPersistence.LoadAllCommittedEvents(GlobalPosition.Start, int.MaxValue, cancellationToken);

            return result;
        }
    }
}