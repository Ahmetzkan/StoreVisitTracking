using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;
using StoreVisitTracking.Application.DTOs.Auth;
using StoreVisitTracking.Application.Interfaces;
using StoreVisitTracking.Domain.Entities;
using StoreVisitTracking.Infrastructure;


namespace StoreVisitTracking.Application.Services;

public class AuthService : IAuthService
{
    private readonly StoreVisitTrackingDbContext _context;
    private readonly IJwtService _jwtService;

    public AuthService(StoreVisitTrackingDbContext context, IJwtService jwtService)
    {
        _context = context;
        _jwtService = jwtService;
    }

    public async Task<AuthResponseDto> RegisterAsync(RegisterDto registerDto)
    {
        if (await _context.Users.AnyAsync(u => u.Username == registerDto.Username))
        {
            throw new Exception("Bu kullanıcı adı zaten kullanılıyor");
        }

        var user = new User
        {
            Id = Guid.NewGuid(),
            Username = registerDto.Username,
            PasswordHash = HashPassword(registerDto.Password),
            Role = registerDto.Role,
            CreatedAt = DateTime.UtcNow
        };

        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();

        var token = _jwtService.GenerateToken(user);
        return new AuthResponseDto
        {
            Token = token,
            Username = user.Username,
            Role = user.Role,
            Expiration = DateTime.UtcNow.AddMinutes(60)
        };
    }

    public async Task<AuthResponseDto> LoginAsync(LoginDto loginDto)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == loginDto.Username);
        if (user == null || !VerifyPassword(loginDto.Password, user.PasswordHash))
        {
            throw new Exception("Kullanıcı adı veya şifre hatalı");
        }

        var token = _jwtService.GenerateToken(user);
        return new AuthResponseDto
        {
            Token = token,
            Username = user.Username,
            Role = user.Role,
            Expiration = DateTime.UtcNow.AddMinutes(60)
        };
    }

    private string HashPassword(string password)
    {
        using var sha256 = SHA256.Create();
        var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
        return Convert.ToBase64String(hashedBytes);
    }

    private bool VerifyPassword(string password, string hash)
    {
        return HashPassword(password) == hash;
    }
}