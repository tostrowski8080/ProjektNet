using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using WorkshopManager.DTOs;
using WorkshopManager.Models;

namespace WorkshopManager.Pages.Account
{
    public class LoginModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public LoginModel(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        [BindProperty]
        public LoginInput Input { get; set; }

        public class LoginInput
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; }

            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            var result = await _signInManager.PasswordSignInAsync(Input.Email, Input.Password, false, false);

            if (!result.Succeeded)
            {
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                return Page();
            }

            var user = await _userManager.FindByEmailAsync(Input.Email);
            var roles = await _userManager.GetRolesAsync(user);

            return roles.FirstOrDefault() switch
            {
                "Admin" => RedirectToPage("/Dashboard/Admin"),
                "Mechanic" => RedirectToPage("/Dashboard/Mechanic"),
                "Receptionist" => RedirectToPage("/Dashboard/Receptionist"),
                "Client" => RedirectToPage("/Dashboard/Client"),
                _ => RedirectToPage("/Index"),
            };
        }
    }
}
