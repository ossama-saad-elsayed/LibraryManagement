using LibraryManagement.DTOS;
using LibraryManagement.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace LibraryManagement.Services
{
    public class OverdueFineScanner : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly FineOptions _settings;
        private readonly ILogger<OverdueFineScanner> _logger;

        public OverdueFineScanner(IServiceScopeFactory scopeFactory,
            IOptions<FineOptions> options, ILogger<OverdueFineScanner> logger)
        {
            _scopeFactory = scopeFactory;
            _settings = options.Value;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await ScanAsync(stoppingToken);
                }
                catch (Exception ex) when (ex is not OperationCanceledException)
                {
                    _logger.LogError(ex, "Overdue fine scan failed");
                }

                try
                {
                    await Task.Delay(TimeSpan.FromHours(_settings.ScannerIntervalHours), stoppingToken);
                }
                catch (OperationCanceledException) { break; }
            }
        }

        private async Task ScanAsync(CancellationToken ct)
        {
            using var scope = _scopeFactory.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<LibraryManagementDbContext>();

            var now = DateTime.UtcNow;
            var overdueRecords = await db.BorrowRecords
                .Where(b => b.Status == "Borrowed" && b.DueDate < now)
                .Where(b => !db.Fines.Any(f => f.BorrowRecordId == b.Id && !f.IsPaid)).ToListAsync(ct);

            foreach (var record in overdueRecords)
            {
                var amount = BorrowRecordService.CalculateOverdueAmount(record.DueDate, now, _settings);
                if (amount.HasValue)
                    db.Fines.Add(new Fine
                    {
                        BorrowRecordId = record.Id,
                        Amount = amount.Value,
                        IsPaid = false
                    });
            }

            if (overdueRecords.Count > 0)
            {
                await db.SaveChangesAsync(ct);
                _logger.LogInformation("Created fines for {Count} overdue records", overdueRecords.Count);
            }
        }
    }
}
