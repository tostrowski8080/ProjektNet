using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using WorkshopManager.Models;

namespace WorkshopManager.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public RegisterModel(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [BindProperty(SupportsGet = true)]
        public RegisterInput Input { get; set; } = new RegisterInput();

        public class RegisterInput
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; }

            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            [Required]
            [Compare("Password")]
            public string ConfirmPassword { get; set; }

            [Required]
            public string Username { get; set; }
        }

        public async Task<IActionResult> OnPostAsync()
        {

            if (!ModelState.IsValid)
            {
                foreach (var kvp in ModelState)
                {
                    foreach (var error in kvp.Value.Errors)
                    {
                        Console.WriteLine($"Validation error on {kvp.Key}: {error.ErrorMessage}");
                    }
                }
                return Page();
            }

            if (Input.Password != Input.ConfirmPassword)
            {
                Console.WriteLine($"Password");
                return Page();
            }
            var user = new ApplicationUser { UserName = Input.Username, Email = Input.Email, UserRole = "Client" };
            var result = await _userManager.CreateAsync(user, Input.Password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "Client");
                await _signInManager.SignInAsync(user, isPersistent: false);
                Console.WriteLine($"Success");
                return RedirectToRoleDashboard("Client");
            }

            foreach (var error in result.Errors)
            {
                Console.WriteLine($"Identity error: {error.Code} - {error.Description}");
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return Page();
        }

        private IActionResult RedirectToRoleDashboard(string role)
        {
            return role switch
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
