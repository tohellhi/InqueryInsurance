using Core.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Core.Infrastructures;

public class AppDbContext : IdentityDbContext<User, IdentityRole<long>, long>
{
    public AppDbContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<InsurancePattern> InsurancePatterns { get; set; }
    public DbSet<Request> Request { get; set; }


    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<InsurancePattern>().Property(x => x.PercentOfNetPremium).HasColumnType("decimal(5, 4)");
        builder.Entity<InsurancePattern>().HasData(new List<InsurancePattern>()
        {
            new InsurancePattern(){Id=1,Title="پوشش جراحی",MinFund=5000,MaxFund=500000000,PercentOfNetPremium=0.0052M},
            new InsurancePattern(){Id=2,Title="پوشش فوت دندانزشکی",MinFund=4000,MaxFund=400000000,PercentOfNetPremium=0.0042M},
            new InsurancePattern(){Id=3,Title="پوشش بستری",MinFund=2000,MaxFund=200000000,PercentOfNetPremium=0.005M}
        });

        base.OnModelCreating(builder);
    }
}