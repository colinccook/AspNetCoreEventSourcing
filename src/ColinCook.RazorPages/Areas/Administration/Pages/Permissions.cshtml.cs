using ColinCook.RazorPages.Helpers;
using EventFlow;
using EventFlow.Queries;

namespace ColinCook.RazorPages.Areas.Administration.Pages
{
    public class PermissionsModel : BasePageModel
    {
        public PermissionsModel(IQueryProcessor queryProcessor, ICommandBus commandBus) : base(queryProcessor, commandBus)
        {
        }
    }
}