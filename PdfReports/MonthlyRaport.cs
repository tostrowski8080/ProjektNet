using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using WorkshopManager.Models;

namespace WorkshopManager.PdfReports
{
    public static class MonthlyRaport
    {
        public static byte[] Generate(List<ServiceOrder> orders, List<Vehicle> vehicles, int year, int month)
        {
            var grouped = orders
                .GroupBy(o => o.VehicleId)
                .Select(g =>
                {
                    var first = g.First();
                    return new
                    {
                        VehicleId = g.Key,
                        OrderCount = g.Count(),
                        TotalCost = g.Sum(order =>
                            (order.Tasks?.Sum(t => t.Cost ?? 0) ?? 0) +
                            (int)(order.Tasks?.SelectMany(t => t.Parts ?? new List<Part>()).Sum(p => p.Cost) ?? 0m))
                    };
                })
                .ToList();

            var vehicleDict = vehicles.ToDictionary(v => v.Id);

            return Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(30);

                    page.Header().Text($"Raport napraw – {month:D2}/{year}")
                                 .FontSize(20).Bold();

                    page.Content().Table(table =>
                    {
                        table.ColumnsDefinition(columns =>
                        {
                            columns.RelativeColumn();
                            columns.RelativeColumn();
                            columns.ConstantColumn(80);
                            columns.ConstantColumn(100);
                        });

                        table.Header(header =>
                        {
                            header.Cell().Text("Klient").Bold();
                            header.Cell().Text("Pojazd").Bold();
                            header.Cell().Text("Zleceń").Bold();
                            header.Cell().Text("Suma kosztów").Bold();
                        });

                        foreach (var item in grouped)
                        {
                            if (!vehicleDict.TryGetValue(item.VehicleId, out var vehicle))
                                continue;

                            var client = vehicle.Client;
                            table.Cell().Text($"{client.FirstName} {client.LastName}");
                            table.Cell().Text($"{vehicle.Make} {vehicle.Model}");
                            table.Cell().Text(item.OrderCount.ToString());
                            table.Cell().Text($"{item.TotalCost:C}");
                        }
                    });

                    page.Footer().AlignCenter().Text($"Wygenerowano: {DateTime.Now:yyyy-MM-dd HH:mm}");
                });
            }).GeneratePdf();
        }
    }
}