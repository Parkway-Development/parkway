using ChurchAccountingSystem.Models;
public abstract class Entity
{
    public Guid Id { get; set; }
    public Address Address { get; set; }
    public double? Latitude { get; set; }
    public double? Longitude { get; set; }

    public bool IsVendor { get; set; }
}
