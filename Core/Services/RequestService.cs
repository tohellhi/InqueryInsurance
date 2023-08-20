using Core.Infrastructures;
using Core.Models;
using Microsoft.EntityFrameworkCore;

namespace Core.Services;

public class RequestService
{
    AppDbContext _db;
    InsurancePatternService _insurancePatternService;
    public RequestService(AppDbContext db, InsurancePatternService insurancePatternService)
    {
        _db = db;
        _insurancePatternService = insurancePatternService;
    }

    public async Task<List<Request>> MyRequests(string userId)
    {
        return await _db.Request.Where(x => x.RequesterId == userId).AsNoTracking().ToListAsync();
    }

    public async Task<Request?> MyRequest(string userId, long id)
    {
        return await _db.Request.FirstOrDefaultAsync(x => x.RequesterId == userId && x.Id == id);
    }

    public async Task<long> NewRequests(Request request, List<int> patternsId)
    {
        var patterns = await _insurancePatternService.ListOfInsurancePatterns(patternsId);
        var insurancePayment = await CalculatePermium(patterns, request.Fund);

        if (request.Fund < patterns.Max(x => x.MinFund))
            throw new Exception($"کمترین مبلغ مجاز {patterns.Max(x => x.MinFund)} میباشد");

        request.InsuranceNetPremium = insurancePayment;
        foreach (var item in patternsId)
        {
            request.RequestPatterns.Add(new RequestPattern
            {
                Request = request,
                InsurancePatternId = item
            });
        }
        await _db.Request.AddAsync(request);
        try
        {
            await _db.SaveChangesAsync();
        }
        catch (Exception ex)
        {

            throw;
        }

        return request.Id;
    }

    async Task<decimal> CalculatePermium(List<InsurancePattern> patterns, decimal requestAmount)
    {
        return patterns.Sum(x => x.PercentOfNetPremium * requestAmount);
    }
}