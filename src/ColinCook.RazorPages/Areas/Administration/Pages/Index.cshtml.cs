using ColinCook.RazorPages.Helpers;
using EventFlow;
using EventFlow.Queries;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ColinCook.RazorPages.Areas.Administration.Pages
{
    public class IndexModel : BasePageModel
    {
        public void OnGet()
        {
        }

        public IndexModel(IQueryProcessor queryProcessor, ICommandBus commandBus) : base(queryProcessor, commandBus)
        {
        }
    }
}