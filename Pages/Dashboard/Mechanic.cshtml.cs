using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WorkshopManager.Pages.Dashboard
{
    [Authorize(Roles = "Mechanic")]
    public class MechanicModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}
