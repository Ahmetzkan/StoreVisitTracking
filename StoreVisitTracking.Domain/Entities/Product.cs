using System;
namespace StoreVisitTracking.Domain.Entities;

public class Product
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Category { get; set; }
    public DateTime CreatedAt { get; set; }
    public Guid StoreId { get; set; }

    public Store Store { get; set; }
    public virtual ICollection<Photo> Photos { get; set; }
}