using AdminDashboardApi.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AdminDashboardApi.Services
{
	public class AuthManager : IAuthManager
	{

		private readonly IConfiguration _configuration;
		private readonly UserManager<ApplicationUser> _userManager;
		private ApplicationUser _user;

		private const string _loginProvider = "AdminDashboard";
		private const string _refreshToken = "RefreshToken";

		public AuthManager(IConfiguration configuration, UserManager<ApplicationUser> userManager)
		{
			_configuration = configuration;
			_userManager = userManager;
		}
		public async Task<AuthResponse?> Login(LoginDto loginDto)
		{
			_user = await _userManager.FindByEmailAsync(loginDto.Email);
			bool isValidUser = await _userManager.CheckPasswordAsync(_user, loginDto.Password);

			if (_user == null || isValidUser == false)
			{
				return null;
			}

			var token = await GenerateToken();

			return new AuthResponse
			{
				Token = token,
				UserId = _user.Id,
				RefreshToken = await CreateRefreshToken()

			};
		}

		public async Task<string> GenerateToken()
		{
			var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:Key"]));
			var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

			var roles = await _userManager.GetRolesAsync(_user);

			var roleClaims = roles.Select(x => new Claim(ClaimTypes.Role, x)).ToList();
			var userClaims = await _userManager.GetClaimsAsync(_user);

			var claims = new List<Claim>
            {
	        new Claim(JwtRegisteredClaimNames.Sub, _user.Email),
	        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
	        new Claim(JwtRegisteredClaimNames.Email, _user.Email),
	        new Claim("uid", _user.Id),
            }
			.Union(userClaims).Union(roleClaims);

			var token = new JwtSecurityToken(
			issuer: _configuration["JwtSettings:Issuer"],
	     	audience: _configuration["JwtSettings:Audience"],
		    claims: claims,
		    expires: DateTime.UtcNow.AddMinutes(Convert.ToInt32(_configuration["JwtSettings:DurationInMinutes"])),
		    signingCredentials: credentials);

			return new JwtSecurityTokenHandler().WriteToken(token);
		}

		public async Task<AuthResponse> VertifyRefreshToken(AuthResponse request)
		{
			var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
			var tokenContent = jwtSecurityTokenHandler.ReadJwtToken(request.Token);
			var userName = tokenContent.Claims.ToList().FirstOrDefault(q => q.Type == JwtRegisteredClaimNames.Email)?.Value;
			_user = await _userManager.FindByEmailAsync(userName);

			if (_user == null || _user.Id != request.UserId)
			{
				return null;
			}

			var isValidRefreshToken = await _userManager.VerifyUserTokenAsync(_user,
				_loginProvider, _refreshToken, request.RefreshToken);

			if (isValidRefreshToken)
			{
				var token = await GenerateToken();
				return new AuthResponse
				{
					Token = token,
					UserId = _user.Id,
					RefreshToken = await CreateRefreshToken()
				};
			}
			await _userManager.UpdateSecurityStampAsync(_user);
			return null;
		}

		public async Task<string> CreateRefreshToken()
		{
			await _userManager.RemoveAuthenticationTokenAsync(_user, _loginProvider, _refreshToken);
			var newRefreshToken = await _userManager.GenerateUserTokenAsync(_user, _loginProvider, _refreshToken);
			var result = await _userManager.SetAuthenticationTokenAsync(_user, _loginProvider, _refreshToken, newRefreshToken);
			return newRefreshToken;
		
		}	
	}
}

