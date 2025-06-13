using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using System.Net.Mail;
using System.Net;
using WorkshopManager.Data;
using WorkshopManager.PdfReports;
using WorkshopManager.Models;

namespace WorkshopManager.Services
{
    public class OpenOrderReportBackgroundService : BackgroundService
    {
        private readonly IServiceProvider _services;
        private readonly ILogger<OpenOrderReportBackgroundService> _logger;

        public OpenOrderReportBackgroundService(IServiceProvider services, ILogger<OpenOrderReportBackgroundService> logger)
        {
            _services = services;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using (var scope = _services.CreateScope())
                {
                    var context = scope.ServiceProvider.GetRequiredService<WorkshopDbContext>();

                    var openOrders = await context.ServiceOrders.Include(o => o.Tasks!)
                            .ThenInclude(t => t.Parts!)
                            .Where(o => o.Status == ServiceOrder.StatusType.New || o.Status == ServiceOrder.StatusType.InProgress)
                            .ToListAsync();

                    var vehicles = await context.Vehicles.Include(v => v.Client).ToListAsync();

                    var pdfBytes = OpenOrderPdfGenerator.Generate(openOrders, vehicles);
                    var filePath = Path.Combine("wwwroot/reports", "open_orders.pdf");
                    Directory.CreateDirectory("wwwroot/reports");
                    await File.WriteAllBytesAsync(filePath, pdfBytes, stoppingToken);

                    await SendEmailWithAttachment(pdfBytes);
                    _logger.LogInformation("Sent: {count}", openOrders.Count);
                }

                await Task.Delay(TimeSpan.FromMinutes(60), stoppingToken);
            }
        }

        private async Task SendEmailWithAttachment(byte[] attachment)
        {
            var mail = new MailMessage("noreply@example.com", "admin@admin.com");
            mail.Subject = "Raport";
            mail.Body = "Pdf raport";

            mail.Attachments.Add(new Attachment(new MemoryStream(attachment), "open_orders.pdf"));

            using var smtp = new SmtpClient("smtp.gmail.com", 587)
            {
                Credentials = new NetworkCredential("disdon802@gmail.com", "waap bguf hzby nrwr"),
                EnableSsl = true
            };

            await smtp.SendMailAsync(mail);
        }
    }
}
