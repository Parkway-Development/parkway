namespace ChurchAccountingSystem.Models
{
    public class EmployeeProfile
    {
        public PaySchedule PaySchedule { get; set; }
        public bool IsExternallyManaged { get; set; }
        public decimal? GrossAmount { get; set; }

        public bool IsFullTime { get; set; }
        public bool IsHourly { get; set; }

        // Withholding and benefits TBD
    }
}
