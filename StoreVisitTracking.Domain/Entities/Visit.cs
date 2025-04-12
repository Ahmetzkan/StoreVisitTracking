using System;
using StoreVisitTracking.Domain.Enums;

namespace StoreVisitTracking.Domain.Entities;

public class Visit
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Guid StoreId { get; set; }
    public DateTime VisitDate { get; set; }
    public VisitStatus Status { get; set; }

    public virtual User User { get; set; }
    public virtual Store Store { get; set; }
    public virtual ICollection<Photo> Photos { get; set; } = new List<Photo>();
}