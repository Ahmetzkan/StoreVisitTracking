using StoreVisitTracking.Domain.Enums;

namespace StoreVisitTracking.Application.DTOs.Auth;

public class AuthResponseDto
{
    public string Token { get; set; }
    public string Username { get; set; }
    public UserRole Role { get; set; }
    public DateTime Expiration { get; set; }
} 