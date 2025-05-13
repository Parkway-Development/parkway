public class Contribution
{
    public Guid Id { get; set; }
    public DateTime Date { get; set; }

    // Source
    public Guid EntityId { get; set; }
    public Entity Entity { get; set; }

    // Categorization
    public ContributionType Type { get; set; } // Tithe or Offering
    public ContributionMethod Method { get; set; } // Cash, Check, Card, etc.

    public decimal Amount { get; set; }

    // Family toggle (if true, applies to entire family)
    public bool IsFamilyContribution { get; set; }

    // Fund
    public Guid? FundId { get; set; } // Optional: defaults to general fund
    public Fund? Fund { get; set; }

    // Budget consideration
    public bool IsTitheOfTithe => Type == ContributionType.Tithe;
}
