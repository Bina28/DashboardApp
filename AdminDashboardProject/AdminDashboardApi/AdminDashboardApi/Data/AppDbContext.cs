using AdminDashboardApi.Data.Configurations;
using AdminDashboardApi.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AdminDashboardApi.Data;
public class AppDbContext : IdentityDbContext<ApplicationUser, IdentityRole, string>

{
	public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }


	public DbSet<Client> Clients => Set<Client>();
	public DbSet<Payment> Payments => Set<Payment>();
	public DbSet<ExchangeRate> ExchangeRates => Set<ExchangeRate>();

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		base.OnModelCreating(modelBuilder);
		modelBuilder.ApplyConfiguration(new RoleConfiguration());
	}
}