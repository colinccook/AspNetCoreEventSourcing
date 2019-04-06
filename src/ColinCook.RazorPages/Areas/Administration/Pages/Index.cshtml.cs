using ColinCook.RazorPages.Helpers;
using EventFlow;
using EventFlow.Queries;

namespace ColinCook.RazorPages.Areas.Administration.Pages
{
    public class IndexModel : BasePageModel
    {
        public IndexModel(IQueryProcessor queryProcessor, ICommandBus commandBus) : base(queryProcessor, commandBus)
        {
        }

        public void OnGet()
        {
        }
    }
}