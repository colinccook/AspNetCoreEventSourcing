using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ColinCCook.AspNetCoreEventSourcing.RazorPages.Pages
{
    public class ContactModel : PageModel
    {
        public string Message { get; set; }

        public void OnGet()
        {
            Message = "Your contact page.";
        }
    }
}