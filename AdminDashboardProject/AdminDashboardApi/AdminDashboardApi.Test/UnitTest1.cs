using AdminDashboardApi.Models;
using AdminDashboardApi.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Moq;

namespace AdminDashboardApi.Test
{
	public class UnitTest1
	{
		public class AuthManagerTests
		{
			private readonly Mock<UserManager<ApplicationUser>> _userManagerMock;
			private readonly Mock<IConfiguration> _configurationMock;
			private readonly AuthManager _authManager;

			public AuthManagerTests()
			{
			
				var userStoreMock = new Mock<IUserStore<ApplicationUser>>();
				_userManagerMock = new Mock<UserManager<ApplicationUser>>(
					userStoreMock.Object, null, null, null, null, null, null, null, null);

				_configurationMock = new Mock<IConfiguration>();
				_configurationMock.Setup(c => c["JwtSettings:Key"]).Returns("MySuperSecretJwtKeyThatIsLongEnough123");

				_configurationMock.Setup(c => c["JwtSettings:Issuer"]).Returns("your-issuer");
				_configurationMock.Setup(c => c["JwtSettings:Audience"]).Returns("your-audience");
				_configurationMock.Setup(c => c["JwtSettings:DurationInMinutes"]).Returns("10");

				_authManager = new AuthManager(_configurationMock.Object, _userManagerMock.Object);
			}

			[Fact]
			public async Task Login_WithInvalidUser_ReturnsNull()
			{
				// Arrange
				_userManagerMock.Setup(u => u.FindByEmailAsync(It.IsAny<string>()))
					.ReturnsAsync((ApplicationUser)null);

				// Act
				var result = await _authManager.Login(new LoginDto { Email = "test@test.com", Password = "password" });

				// Assert
				Assert.Null(result);
			}

			[Fact]
			public async Task Login_WithValidUser_ReturnsAuthResponse()
			{
				// Arrange
				var testUser = new ApplicationUser { Id = "1", Email = "test@test.com" };
				_userManagerMock.Setup(u => u.FindByEmailAsync(It.IsAny<string>()))
					.ReturnsAsync(testUser);
				_userManagerMock.Setup(u => u.CheckPasswordAsync(testUser, It.IsAny<string>()))
					.ReturnsAsync(true);

				// Мокаем роли и claims, если нужно (тут просто пустые списки)
				_userManagerMock.Setup(u => u.GetRolesAsync(testUser))
					.ReturnsAsync(new List<string>());
				_userManagerMock.Setup(u => u.GetClaimsAsync(testUser))
					.ReturnsAsync(new List<System.Security.Claims.Claim>());

				// Act
				var result = await _authManager.Login(new LoginDto { Email = "test@test.com", Password = "password" });

				// Assert
				Assert.NotNull(result);
				Assert.NotNull(result.Token);
				Assert.Equal(testUser.Id, result.UserId);
			}
			[Fact]
			public async Task CreateRefreshToken_ShouldReturnNewRefreshToken()
			{
				// Arrange
				var mockUserManager = new Mock<UserManager<ApplicationUser>>(
					Mock.Of<IUserStore<ApplicationUser>>(), null, null, null, null, null, null, null, null);

				var testUser = new ApplicationUser { Id = "user123" };

			
				mockUserManager
					.Setup(x => x.RemoveAuthenticationTokenAsync(testUser, It.IsAny<string>(), It.IsAny<string>()))
					.ReturnsAsync(IdentityResult.Success);

				mockUserManager
					.Setup(x => x.GenerateUserTokenAsync(testUser, It.IsAny<string>(), It.IsAny<string>()))
					.ReturnsAsync("new-refresh-token");

			
				mockUserManager
					.Setup(x => x.SetAuthenticationTokenAsync(testUser, It.IsAny<string>(), It.IsAny<string>(), "new-refresh-token"))
					.ReturnsAsync(IdentityResult.Success);


				var mockConfig = new Mock<IConfiguration>();
				var authManager = new AuthManager(mockConfig.Object, mockUserManager.Object);

				var userField = typeof(AuthManager).GetField("_user", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
				userField.SetValue(authManager, testUser);

				// Act
				var refreshToken = await authManager.CreateRefreshToken();

				// Assert
				Assert.Equal("new-refresh-token", refreshToken);

				mockUserManager.Verify(x => x.RemoveAuthenticationTokenAsync(testUser, It.IsAny<string>(), It.IsAny<string>()), Times.Once);
				mockUserManager.Verify(x => x.GenerateUserTokenAsync(testUser, It.IsAny<string>(), It.IsAny<string>()), Times.Once);
				mockUserManager.Verify(x => x.SetAuthenticationTokenAsync(testUser, It.IsAny<string>(), It.IsAny<string>(), "new-refresh-token"), Times.Once);
			}

		}
	}
}