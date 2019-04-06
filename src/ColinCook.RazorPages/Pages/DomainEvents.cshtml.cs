using System.Threading;
using System.Threading.Tasks;
using ColinCCook.AspNetCoreEventSourcing.EventFlow.Queries;
using ColinCCook.AspNetCoreEventSourcing.RazorPages.Helpers;
using EventFlow;
using EventFlow.EventStores;
using EventFlow.Queries;

namespace ColinCCook.AspNetCoreEventSourcing.RazorPages.Pages
{
    public class DomainEvents : BasePageModel
    {
        public DomainEvents(IQueryProcessor queryProcessor, ICommandBus commandBus) : base(queryProcessor, commandBus)
        {
        }

        public AllCommittedEventsPage DomainEventsPage { get; set; }

        public async Task OnGetAsync()
        {
            DomainEventsPage = await QueryProcessor.ProcessAsync(
                new GetAllDomainEventsQuery(), CancellationToken.None);
        }
    }
}