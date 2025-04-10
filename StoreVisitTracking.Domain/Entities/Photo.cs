using StoreVisitTracking.Domain.Entities;

public class Photo
{
    public Guid Id { get; set; }
    public Guid VisitId { get; set; }
    public Guid ProductId { get; set; }
    public string Base64Image { get; set; }
    public DateTime UploadedAt { get; set; }

    public virtual Visit Visit { get; set; }
    public virtual Product Product { get; set; }
}
