using StoreVisitTracking.Domain.Entities;

public class Store
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Location { get; set; }
    public DateTime CreatedAt { get; set; }

    public virtual ICollection<Visit> Visits { get; set; } = new List<Visit>();
    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}