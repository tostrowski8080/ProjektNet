using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using WorkshopManager.Data;
using WorkshopManager.PdfReports;
using WorkshopManager.Models;
using static NuGet.Packaging.PackagingConstants;

namespace WorkshopManager.Pages.Clients
{
    public class MonthlyPdfRaportModel : PageModel
    {
        private readonly WorkshopDbContext _context;

        public MonthlyPdfRaportModel(WorkshopDbContext context)
        {
            _context = context;
        }

        [BindProperty(SupportsGet = true)]
        public int SelectedMonth { get; set; } = DateTime.Now.Month;

        [BindProperty(SupportsGet = true)]
        public int SelectedYear { get; set; } = DateTime.Now.Year;

        public async Task<IActionResult> OnGetAsync()
        {
            if (!ModelState.IsValid || SelectedMonth <= 0 || SelectedYear <= 0)
                return Page();

            var orders = await _context.ServiceOrders
                .Include(o => o.Tasks).ThenInclude(t => t.Parts)
                .Where(o => o.CreationDate.Month == SelectedMonth && o.CreationDate.Year == SelectedYear)
                .ToListAsync();

            var vehicles = await _context.Vehicles
                .Include(v => v.Client)
                .ToListAsync();
            var pdf = MonthlyRaport.Generate(orders, vehicles, SelectedYear, SelectedMonth);

            return File(pdf, "application/pdf", $"Raport_napraw_{SelectedYear}_{SelectedMonth:D2}.pdf");
        }
    }
}
