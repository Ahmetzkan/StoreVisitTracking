using StoreVisitTracking.Application.DTOs.Auth;

namespace StoreVisitTracking.Application.Interfaces;

public interface IAuthService
{
    //Task<AuthResponseDto> RegisterAsync(RegisterDto registerDto);
    Task<AuthResponseDto> LoginAsync(LoginDto loginDto);
} 