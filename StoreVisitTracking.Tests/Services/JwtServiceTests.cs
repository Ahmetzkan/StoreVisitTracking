using Microsoft.Extensions.Options;
using Moq;
using StoreVisitTracking.Application.Services;
using StoreVisitTracking.Application.Settings;
using StoreVisitTracking.Domain.Entities;
using StoreVisitTracking.Domain.Enums;
using System.Linq.Dynamic.Core.Tokenizer;
using Xunit;

namespace StoreVisitTracking.Tests.Services
{
    public class JwtServiceTests
    {
        private readonly JwtService _jwtService;
        private readonly JwtSettings _jwtSettings;

        public JwtServiceTests()
        {
            _jwtSettings = new JwtSettings
            {
                SecretKey = "your_very_long_and_secure_jwt_secret_key_at_least_32_chars",
                Issuer = "TestIssuer",
                Audience = "TestAudience",
                ExpirationInMinutes = 60
            };

            var mockOptions = new Mock<IOptions<JwtSettings>>();
            mockOptions.Setup(x => x.Value).Returns(_jwtSettings);

            _jwtService = new JwtService(mockOptions.Object);
        }

        [Fact]
        public void Generate_Token_ValidUser_ValidToken_Returns()
        {
            var user = new User
            {
                Id = Guid.NewGuid(),
                Username = "testuser",
                Role = UserRole.Standard
            };

            var token = _jwtService.GenerateToken(user);

            Assert.NotNull(token);
            Assert.True(_jwtService.ValidateToken(token));
        }

        [Fact]
        public void ValidateToken_InvalidToken_FalseDone()
        {
            var invalidToken = "invalid.token.here";

            var isValid = _jwtService.ValidateToken(invalidToken);

            Assert.False(isValid);
        }

        [Fact]
        public void GetUsernameFromToken_ValidToken_UsernameFrom()
        {
            var user = new User
            {
                Id = Guid.NewGuid(),
                Username = "testuser",
                Role = UserRole.Standard
            };
            var token = _jwtService.GenerateToken(user);

            var username = _jwtService.GetUsernameFromToken(token);

            Assert.Equal(user.Username, username);
        }

        [Fact]
        public void GetRoleFromToken_ValidToken_RoleFrom()
        {
            var user = new User
            {
                Id = Guid.NewGuid(),
                Username = "testuser",
                Role = UserRole.Admin
            };
            var token = _jwtService.GenerateToken(user);

            var role = _jwtService.GetRoleFromToken(token);

            Assert.Equal(user.Role, role);
        }
    }
} 