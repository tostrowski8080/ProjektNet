using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WorkshopManager.Pages.Dashboard
{
    [Authorize(Roles = "Receptionist")]
    public class ReceptionistModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}
