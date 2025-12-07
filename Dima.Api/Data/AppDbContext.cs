using System.Reflection;
using Dima.Api.Models;
using Dima.Core.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Dima.Api.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) 
    : IdentityDbContext
        <
            User, 
            IdentityRole<long>, 
            long,
            IdentityUserClaim<long>,
            IdentityUserRole<long>,
            IdentityUserLogin<long>,
            IdentityRoleClaim<long>,
            IdentityUserToken<long> // Precisa setar todos, pois o tipo do IdentityUser é customizado como long
        >(options) //Primary constructor c# 8 ou superior
{
    public DbSet<Category> Categories { get; set; } = null!;
    public DbSet<Transaction> Transactions { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}