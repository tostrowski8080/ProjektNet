using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using WorkshopManager.Models;

namespace WorkshopManager.PdfReports
{
    public static class OpenOrderPdfGenerator
    {
        public static byte[] Generate(List<ServiceOrder> orders, List<Vehicle> vehicles)
        {
            var vehicleDict = vehicles
                .GroupBy(v => v.Id)
                .Select(g => g.First())
                .ToDictionary(v => v.Id);

            try
            {
                return Document.Create(container =>
                {
                    container.Page(page =>
                    {
                        page.Size(PageSizes.A4);
                        page.Margin(30);
                        page.Header().Text("Open service orders").FontSize(20).Bold();

                        page.Content().Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                            });

                            table.Header(header =>
                            {
                                header.Cell().Text("Client").Bold();
                                header.Cell().Text("Vehicle").Bold();
                                header.Cell().Text("Status").Bold();
                                header.Cell().Text("Cost").Bold();
                            });

                            foreach (var order in orders)
                            {
                                if (!vehicleDict.TryGetValue(order.VehicleId, out var v))
                                    continue;

                                var client = v.Client ?? new Client();
                                var firstName = string.IsNullOrWhiteSpace(client.FirstName) ? "-" : client.FirstName;
                                var lastName = string.IsNullOrWhiteSpace(client.LastName) ? "-" : client.LastName;
                                var clientName = $"{firstName} {lastName}";

                                var make = string.IsNullOrWhiteSpace(v.Make) ? "-" : v.Make;
                                var model = string.IsNullOrWhiteSpace(v.Model) ? "-" : v.Model;
                                var vehicleName = $"{make} {model}";

                                var tasks = order.Tasks ?? new List<ServiceTask>();

                                var labor = tasks.Sum(t => t.Cost ?? 0);
                                var parts = tasks
                                    .SelectMany(t => t.Parts ?? Enumerable.Empty<Part>())
                                    .Sum(p => (p?.Cost ?? 0) * (p?.Quantity ?? 0));

                                var total = labor + parts;
                                var statusText = order.Status.ToString();
                                var costText = $"{total:0.00}";

                                try
                                {
                                    table.Cell().Text(clientName);
                                    table.Cell().Text(vehicleName);
                                    table.Cell().Text(statusText);
                                    table.Cell().Text(costText);
                                }
                                catch (Exception ex)
                                {
                                    table.Cell().Text("ERROR");
                                    table.Cell().Text("ERROR");
                                    table.Cell().Text("ERROR");
                                    table.Cell().Text("ERROR");

                                    Console.WriteLine($"PDF row rendering failed: {ex.Message}");
                                }
                            }
                        });
                    });
                }).GeneratePdf();
            }
            catch (Exception ex)
            {
                Console.WriteLine("PDF generation failed: " + ex);
                throw;
            }
        }
    }
}