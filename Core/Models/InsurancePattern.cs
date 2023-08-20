namespace Core.Models;

public class InsurancePattern
{
    public int Id { get; set; }
    public string Title { get; set; }
    public long MinFund { get; set; }
    public long MaxFund { get; set; }
    public decimal PercentOfNetPremium { get; set; }
}
