public class Fund
{
    public Guid Id { get; set; }
    public string Name { get; set; }

    public Guid? ParentFundId { get; set; } // For grouping under umbrella fund
    public Fund? ParentFund { get; set; }

    public ICollection<Fund> SubFunds { get; set; } = new List<Fund>();
}
