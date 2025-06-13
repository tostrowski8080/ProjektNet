using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using WorkshopManager.Models;

namespace WorkshopManager.PdfReports
{
    public class ClientReport
    {
        public static byte[] Generate(Client client)
        {
            return Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(30);
                    page.Header().Text($"Client services raport: {client.FirstName} {client.LastName}")
                                 .FontSize(20).Bold();

                    page.Content().Column(column =>
                    {
                        foreach (var vehicle in client.Vehicles)
                        {
                            column.Item().PaddingVertical(5).Text($"{vehicle.Make} {vehicle.Model} ({vehicle.RegistrationNumber})")
                                         .FontSize(16).Bold();

                            foreach (var order in vehicle.ServiceOrders)
                            {
                                int labor = order.Tasks?.Sum(t => t.Cost ?? 0) ?? 0;
                                decimal parts = order.Tasks?
                                    .SelectMany(t => t.Parts ?? new List<Part>())
                                    .Sum(p => p.Cost) ?? 0m;

                                decimal total = labor + parts;

                                column.Item().PaddingLeft(10).Text($"Order #{order.Id} - Status: {order.Status} - Cost: {total:C}");
                            }
                        }
                    });

                    page.Footer().AlignCenter().Text("Workshop © 2025");
                });
            }).GeneratePdf();
        }
    }
}