namespace Core.Models;

public class Request
{
    public long Id { get; set; }
    public string Title { get; set; }
    public string RequesterId { get; set; }
    public DateTime DateTime { get; set; }=DateTime.Now;
    public long Amount { get; set; }
    public decimal InsuranceNetPremium { get; set; }
    public User Requester { get; set; }
    public List<RequestPattern> RequestPatterns { get; set; }
}

public class RequestPattern
{
    public long Id { get; set; }
    public long RequestId { get; set; }
    public int InsurancePatternId { get; set; }


    public Request Request { get; set; }
    public InsurancePattern InsurancePattern { get; set; }
}
