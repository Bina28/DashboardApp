using Microsoft.AspNetCore.Identity;

namespace AdminDashboardApi.Models;

public class ApplicationUser: IdentityUser
{
	public string FullName { get; set; }
}
