using AdminDashboardApi.Models;
using Microsoft.AspNetCore.Identity;

namespace AdminDashboardApi.Data;

	public static class SeedData
	{
		public static async Task SeedDatabaseAsync(AppDbContext db, UserManager<ApplicationUser> userManager)
		{
			if (!userManager.Users.Any())
			{
				var adminUser = new ApplicationUser
				{
					UserName = "admin@mirra.dev",
					Email = "admin@mirra.dev",
					FullName = "Admin"
				};
				var adminPassword = "admin123";

				var result = await userManager.CreateAsync(adminUser, adminPassword);
				if (!result.Succeeded)
					throw new Exception($"Kunne ikke opprette admin-bruker: {string.Join(", ", result.Errors.Select(e => e.Description))}");

				
				await userManager.AddToRoleAsync(adminUser, "Administrator");
			}

			
			if (!db.Clients.Any())
			{
				var clients = new[]
				{
				new Client { Name = "Alice", Email = "alice@example.com" },
				new Client { Name = "Bob", Email = "bob@example.com" },
				new Client { Name = "Charlie", Email = "charlie@example.com" }
			};

				db.Clients.AddRange(clients);
				await db.SaveChangesAsync();

				var payments = new[]
				{
				new Payment { ClientId = clients[0].Id, Amount = 100, Date = DateTime.UtcNow.AddDays(-1) },
				new Payment { ClientId = clients[1].Id, Amount = 150, Date = DateTime.UtcNow.AddDays(-2) },
				new Payment { ClientId = clients[0].Id, Amount = 200, Date = DateTime.UtcNow.AddDays(-3) },
				new Payment { ClientId = clients[2].Id, Amount = 80, Date = DateTime.UtcNow.AddDays(-4) },
				new Payment { ClientId = clients[1].Id, Amount = 120, Date = DateTime.UtcNow.AddDays(-5) }
			};

				db.Payments.AddRange(payments);

				db.ExchangeRates.Add(new ExchangeRate { Rate = 10M, UpdatedAt = DateTime.UtcNow });

				await db.SaveChangesAsync();
			}
		}
	

}
