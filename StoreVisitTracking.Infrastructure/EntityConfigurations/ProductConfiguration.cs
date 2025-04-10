using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StoreVisitTracking.Domain.Entities;

namespace StoreVisitTracking.Infrastructure.EntityConfigurations
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.ToTable("Products");

            builder.HasKey(p => p.Id);
            builder.Property(p => p.Id).HasColumnName("Id").IsRequired();

            builder.Property(p => p.Name)
                   .HasColumnName("Name")
                   .IsRequired()
                   .HasMaxLength(200); 

            builder.Property(p => p.Category)
                   .HasColumnName("Category")
                   .IsRequired()
                   .HasMaxLength(100); 

            builder.Property(p => p.CreatedAt)
                   .HasColumnName("CreatedAt")
                   .IsRequired();  

            builder.Property(p => p.StoreId)
                   .HasColumnName("StoreId")
                   .IsRequired();  

            builder.HasOne(p => p.Store)
                   .WithMany(s => s.Products)
                   .HasForeignKey(p => p.StoreId)
                   .OnDelete(DeleteBehavior.Restrict);  

            builder.HasMany(p => p.Photos)
                   .WithOne(photo => photo.Product)
                   .HasForeignKey(photo => photo.ProductId)
                   .OnDelete(DeleteBehavior.Cascade);  
        }
    }
}
