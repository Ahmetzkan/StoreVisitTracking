using StoreVisitTracking.Domain.Entities;
using StoreVisitTracking.Domain.Enums;

namespace StoreVisitTracking.Application.Services;

public interface IJwtService
{
    string GenerateToken(User user);
    bool ValidateToken(string token);
    string GetUsernameFromToken(string token);
    UserRole GetRoleFromToken(string token);
} 