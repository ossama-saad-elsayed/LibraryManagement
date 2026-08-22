namespace LibraryManagement.DTOS
{
    public class FineOptions
    {
        public decimal DailyFineRate { get; set; } = 1.00m;
        public int GracePeriodDays { get; set; } = 0;
        public decimal MaxFineCap { get; set; } = 100.00m;
        public int ScannerIntervalHours { get; set; } = 24;
        public bool BlockBorrowingOnUnpaidFines { get; set; } = true;
    }
}
