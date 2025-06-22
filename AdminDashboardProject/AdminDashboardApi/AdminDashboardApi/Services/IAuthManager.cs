using AdminDashboardApi.Models;

namespace AdminDashboardApi.Services;

public interface IAuthManager
{	Task<AuthResponse> Login(LoginDto loginDto);
	Task<string> CreateRefreshToken();
	Task<AuthResponse> VertifyRefreshToken(AuthResponse response);
}
