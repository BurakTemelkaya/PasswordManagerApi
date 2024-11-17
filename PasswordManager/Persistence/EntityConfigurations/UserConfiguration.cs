using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.EntityConfigurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users").HasKey(u => u.Id);

        builder.Property(u => u.Id).HasColumnName("Id").IsRequired();
        builder.Property(u => u.UserName).HasColumnName("UserName").HasMaxLength(32).IsRequired();
        builder.Property(u => u.Email).HasColumnName("Email").HasMaxLength(254).IsRequired();
        builder.Property(u => u.MasterPasswordSalt).HasColumnName("MasterPasswordSalt").IsRequired();
        builder.Property(u => u.MasterPasswordHash).HasColumnName("MasterPasswordHash").IsRequired();
        builder.Property(u => u.CreatedDate).HasColumnName("CreatedDate").IsRequired();
        builder.Property(u => u.UpdatedDate).HasColumnName("UpdateDate");
        builder.Property(u => u.DeletedDate).HasColumnName("DeletedDate");

        builder.HasMany(u => u.Passwords);

        builder.HasQueryFilter(u => !u.DeletedDate.HasValue);

    }
}
