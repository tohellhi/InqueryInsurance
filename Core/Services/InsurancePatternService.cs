using Core.Infrastructures;
using Core.Models;
using Microsoft.EntityFrameworkCore;

namespace Core.Services;

public class InsurancePatternService
{
    AppDbContext _db;

    public InsurancePatternService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<List<InsurancePattern>> ListOfInsurancePatterns()
    {
        return await _db.InsurancePatterns.AsNoTracking().ToListAsync();
    }

    public async Task<List<InsurancePattern>> ListOfInsurancePatterns(List<int> patternsId)
    {
        return await _db.InsurancePatterns.Where(x=>patternsId.Contains(x.Id)).AsNoTracking().ToListAsync();
    }
    
}
