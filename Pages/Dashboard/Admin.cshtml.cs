using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WorkshopManager.Models;

namespace WorkshopManager.Pages.Dashboard
{
    [Authorize(Roles = "Admin")]
    public class AdminModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AdminModel(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public List<ApplicationUser> Users { get; set; } = new();
        public Dictionary<string, string> UserRoles { get; set; } = new();

        [BindProperty]
        public string SelectedUserId { get; set; }

        [BindProperty]
        public string SelectedRole { get; set; }

        public async Task OnGetAsync()
        {
            Users = _userManager.Users.ToList();
            foreach (var user in Users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                UserRoles[user.Id] = roles.FirstOrDefault() ?? "Brak";
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (string.IsNullOrEmpty(SelectedUserId) || string.IsNullOrEmpty(SelectedRole))
                return RedirectToPage();

            var user = await _userManager.FindByIdAsync(SelectedUserId);
            if (user == null) return NotFound();
            user.UserRole = SelectedRole;

            var currentRole = await _userManager.GetRolesAsync(user);
            await _userManager.RemoveFromRolesAsync(user, currentRole);
            await _userManager.AddToRoleAsync(user, SelectedRole);

            return RedirectToPage();
        }
    }
}
