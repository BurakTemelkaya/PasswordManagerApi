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
        builder.Property(p=> p.Name).HasColumnName("Name").IsRequired().HasMaxLength(128);
        builder.Property(p=> p.Description).HasColumnName("Description").IsRequired().HasMaxLength(256);
        builder.Property(p=> p.EncryptedPassword).HasColumnName("EncryptedPassword").IsRequired().HasMaxLength(1024);
        builder.Property(p=> p.WebSiteUrl).HasColumnName("Name").HasMaxLength(2000);
        builder.Property(p=> p.CreatedDate).IsRequired().HasColumnName("CreatedDate");
        builder.Property(p=> p.UpdatedDate).HasColumnName("UpdatedDate");
        builder.Property(p=> p.DeletedDate).HasColumnName("DeleteDate");

        builder.HasOne(p => p.User);

        builder.HasQueryFilter(b => !b.DeletedDate.HasValue);
    }
}
