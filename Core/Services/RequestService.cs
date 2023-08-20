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
        return await _db.Request.Where(x=>x.RequesterId==userId).AsNoTracking().ToListAsync();
    }

    public async Task<Request?> MyRequest(string userId,long id)
    {
        return await _db.Request.FirstOrDefaultAsync(x => x.RequesterId == userId && x.Id==id);
    }

    public async Task<long> NewRequests(Request request,List<int> patternsId)
    {
        var patterns=await _insurancePatternService.ListOfInsurancePatterns(patternsId);
        var insurancePayment=patterns.Sum(x=>x.PercentOfNetPremium * request.Amount);

        if (request.Amount < patterns.Max(x => x.MinFund))
            throw new Exception($"کمترین مبلغ مجاز {patterns.Max(x => x.MinFund)} میباشد");

        request.InsuranceNetPremium = insurancePayment;
        await _db.Request.AddAsync(request);
        await _db.SaveChangesAsync();
        return request.Id;
    }
}