using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StoreVisitTracking.Domain.Entities;

namespace StoreVisitTracking.Infrastructure.EntityConfigurations
{
    public class VisitConfiguration : IEntityTypeConfiguration<Visit>
    {
        public void Configure(EntityTypeBuilder<Visit> builder)
        {
            builder.ToTable("Visits");

            builder.HasKey(v => v.Id);
            builder.Property(v => v.Id).HasColumnName("Id").IsRequired();
            builder.Property(v => v.VisitDate).HasColumnName("VisitDate").IsRequired();
            builder.Property(v => v.Status).HasColumnName("Status").IsRequired();

            builder.HasOne(v => v.User)
                   .WithMany(u => u.Visits)
                   .HasForeignKey(v => v.UserId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(v => v.Store)
                   .WithMany(s => s.Visits)
                   .HasForeignKey(v => v.StoreId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(v => v.Photos)
                   .WithOne(p => p.Visit)
                   .HasForeignKey(p => p.VisitId)
                   .OnDelete(DeleteBehavior.Cascade);  
        }
    }
}
