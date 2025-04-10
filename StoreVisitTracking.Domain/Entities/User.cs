using System;
using StoreVisitTracking.Domain.Enums;

namespace StoreVisitTracking.Domain.Entities;

public class User
{
    public Guid Id { get; set; }
    public string Username { get; set; }
    public string PasswordHash { get; set; }
    public UserRole Role { get; set; }
    public DateTime CreatedAt { get; set; }

    public ICollection<Visit> Visits { get; set; } = new List<Visit>();
}