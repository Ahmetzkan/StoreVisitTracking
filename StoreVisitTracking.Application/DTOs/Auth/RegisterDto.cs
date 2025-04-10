using StoreVisitTracking.Domain.Enums;

namespace StoreVisitTracking.Application.DTOs.Auth;

public class RegisterDto
{
    public string Username { get; set; }
    public string Password { get; set; }
    public string ConfirmPassword { get; set; }
    public UserRole Role { get; set; } = UserRole.Standard;
} 