using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StoreVisitTracking.Domain.Entities;

namespace StoreVisitTracking.Infrastructure.EntityConfigurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users");

            builder.HasKey(u => u.Id);
            builder.Property(u => u.Id).HasColumnName("Id").IsRequired();
            builder.Property(u => u.Username).HasColumnName("Username").IsRequired().HasMaxLength(100);
            builder.Property(u => u.PasswordHash).HasColumnName("PasswordHash").IsRequired();
            builder.Property(u => u.Role).HasColumnName("Role").IsRequired();

            builder.HasIndex(u => u.Username).IsUnique(); 
        }
    }
}
