using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ColinCCook.AspNetCoreEventSourcing.RazorPages.Pages
{
    public class AboutModel : PageModel
    {
        public string Message { get; set; }

        public void OnGet()
        {
            Message = "Your application description page.";
        }
    }
}