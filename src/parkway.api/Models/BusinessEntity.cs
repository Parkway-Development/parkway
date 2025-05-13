namespace ChurchAccountingSystem.Models
{
    public class BusinessEntity : Entity
    {
        public string BusinessName { get; set; }
        public Guid PrimaryContactId { get; set; } // Must link to IndividualEntity
    }
}
