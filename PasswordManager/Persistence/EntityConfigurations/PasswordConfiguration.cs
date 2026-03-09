using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.EntityConfigurations;

public class PasswordConfiguration : IEntityTypeConfiguration<Password>
{
    public void Configure(EntityTypeBuilder<Password> builder)
    {
        builder.ToTable("Passwords").HasKey(p => p.Id);

        builder.Property(p => p.Id).HasColumnName("Id").IsRequired();
        builder.Property(p => p.EncryptedName).HasColumnName("Name").IsRequired().HasMaxLength(2048);
        builder.Property(p => p.EncryptedDescription).HasColumnName("Description").HasMaxLength(2048);
        builder.Property(p => p.EncryptedPassword).HasColumnName("EncryptedPassword").IsRequired().HasMaxLength(2048);
        builder.Property(p => p.EncryptedWebSiteUrl).HasColumnName("WebSiteUrl").HasMaxLength(2048);
        builder.Property(p => p.CreatedDate).IsRequired().HasColumnName("CreatedDate");
        builder.Property(p => p.UpdatedDate).HasColumnName("UpdatedDate");
        builder.Property(p => p.DeletedDate).HasColumnName("DeleteDate");
        builder.Property(p => p.UserId).HasColumnName("UserId").IsRequired();
        builder.Property(p => p.Iv).HasColumnName("Iv").HasMaxLength(32).IsRequired();

        builder.HasOne(p => p.User)
            .WithMany(u => u.Passwords)
            .HasForeignKey(p => p.UserId);

        builder.HasQueryFilter(b => !b.DeletedDate.HasValue);
    }
}
