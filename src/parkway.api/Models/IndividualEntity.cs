namespace ChurchAccountingSystem.Models
{
    public class IndividualEntity : Entity
    {
        public int MemberId { get; set; } // starts at 1000
        public string FirstName { get; set; }
        public string? MiddleName { get; set; }
        public string LastName { get; set; }
        public string? Suffix { get; set; }

        public string Gender { get; set; }
        public DateTime? BirthDate { get; set; }

        public string Email { get; set; } // must be valid
        public string? MobileNumber { get; set; }

        public Guid? FamilyId { get; set; }
        public bool IsMember { get; set; }
        public EntityStatus Status { get; set; }

        public bool IsEmployee { get; set; }
        public bool IsContractor { get; set; }

        public EmployeeProfile? EmployeeDetails { get; set; }
    }
}
