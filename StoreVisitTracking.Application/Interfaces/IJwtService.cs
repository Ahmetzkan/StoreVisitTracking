using StoreVisitTracking.Domain.Entities;
using StoreVisitTracking.Domain.Enums;

namespace StoreVisitTracking.Application.Interfaces;

public interface IJwtService
{
    string GenerateToken(User user);
    bool ValidateToken(string token);
    string GetUsernameFromToken(string token);
    UserRole GetRoleFromToken(string token);
} 