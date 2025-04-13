using Microsoft.EntityFrameworkCore;
using Moq;
using StoreVisitTracking.Application.DTOs.Auth;
using StoreVisitTracking.Application.Interfaces;
using StoreVisitTracking.Application.Services;
using StoreVisitTracking.Domain.Entities;
using StoreVisitTracking.Domain.Enums;
using StoreVisitTracking.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace StoreVisitTracking.Tests.Services
{
    public class AuthServiceTests : IDisposable
    {
        private readonly StoreVisitTrackingDbContext _context;
        private readonly Mock<IJwtService> _mockJwtService;
        private readonly AuthService _authService;

        public AuthServiceTests()
        {
            var options = new DbContextOptionsBuilder<StoreVisitTrackingDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new StoreVisitTrackingDbContext(options);
            _mockJwtService = new Mock<IJwtService>();
            _authService = new AuthService(_context, _mockJwtService.Object);

            _context.Database.EnsureCreated();
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        [Fact]
        public async Task LoginAsync_CurrentCredentials_TokenDone()
        {
            var loginDto = new LoginDto
            {
                Username = "testuser",
                Password = "testpass"
            };

            var hashedPassword = HashPassword("testpass");
            var existingUser = new User
            {
                Id = Guid.NewGuid(),
                Username = "testuser",
                PasswordHash = hashedPassword,
                Role = UserRole.Standard,
                CreatedAt = DateTime.UtcNow
            };

            await _context.Users.AddAsync(existingUser);
            await _context.SaveChangesAsync();

            _mockJwtService.Setup(x => x.GenerateToken(It.IsAny<User>()))
                .Returns("test_token");

            var result = await _authService.LoginAsync(loginDto);

            Assert.Equal("test_token", result.Token);
            Assert.Equal(loginDto.Username, result.Username);
        }

        [Fact]
        public async Task LoginAsync_Invalid_Credentials_ThrowError()
        {
            var loginDto = new LoginDto
            {
                Username = "testuser",
                Password = "wrongpass"
            };

            var hashedPassword = HashPassword("testpass");
            var existingUser = new User
            {
                Id = Guid.NewGuid(),
                Username = "testuser",
                PasswordHash = hashedPassword,
                Role = UserRole.Standard,
                CreatedAt = DateTime.UtcNow
            };

            await _context.Users.AddAsync(existingUser);
            await _context.SaveChangesAsync();

            await Assert.ThrowsAsync<Exception>(() =>
                _authService.LoginAsync(loginDto));
        }

        private string HashPassword(string password)
        {
            using var sha256 = System.Security.Cryptography.SHA256.Create();
            var hashedBytes = sha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(hashedBytes);
        }
    }
}