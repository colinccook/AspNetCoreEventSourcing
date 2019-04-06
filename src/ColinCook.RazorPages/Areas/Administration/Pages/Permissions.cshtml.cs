using ColinCCook.AspNetCoreEventSourcing.RazorPages.Helpers;
using EventFlow;
using EventFlow.Queries;

namespace ColinCCook.AspNetCoreEventSourcing.RazorPages.Areas.Administration.Pages
{
    public class PermissionsModel : BasePageModel
    {
        public PermissionsModel(IQueryProcessor queryProcessor, ICommandBus commandBus) : base(queryProcessor, commandBus)
        {
        }
    }
}