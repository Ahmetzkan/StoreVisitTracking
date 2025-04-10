using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StoreVisitTracking.Domain.Entities;

namespace StoreVisitTracking.Infrastructure.EntityConfigurations
{
    public class PhotoConfiguration : IEntityTypeConfiguration<Photo>
    {
        public void Configure(EntityTypeBuilder<Photo> builder)
        {
            builder.ToTable("Photos");

            builder.HasKey(p => p.Id);
            builder.Property(p => p.Id).HasColumnName("Id").IsRequired();
            builder.Property(p => p.Base64Image).HasColumnName("Base64Image").IsRequired();
            builder.Property(p => p.UploadedAt).HasColumnName("UploadedAt").IsRequired();

            builder.HasOne(p => p.Visit)
                   .WithMany(v => v.Photos)
                   .HasForeignKey(p => p.VisitId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(p => p.Product)
                   .WithMany()
                   .HasForeignKey(p => p.ProductId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
