using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StoreVisitTracking.Domain.Entities;

namespace StoreVisitTracking.Infrastructure.EntityConfigurations
{
    public class StoreConfiguration : IEntityTypeConfiguration<Store>
    {
        public void Configure(EntityTypeBuilder<Store> builder)
        {
            builder.ToTable("Stores");

            builder.HasKey(s => s.Id);
            builder.Property(s => s.Id).HasColumnName("Id").IsRequired();
            builder.Property(s => s.Name).HasColumnName("Name").IsRequired().HasMaxLength(200);
            builder.Property(s => s.Location).HasColumnName("Location").IsRequired().HasMaxLength(300);
            builder.Property(s => s.CreatedAt).HasColumnName("CreatedAt").IsRequired();

            builder.HasMany(s => s.Visits)
                   .WithOne(v => v.Store)
                   .HasForeignKey(v => v.StoreId);

            builder.HasMany(s => s.Products)
                   .WithOne(p => p.Store)
                   .HasForeignKey(p => p.StoreId);
        }
    }
}
