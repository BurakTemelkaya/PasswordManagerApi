using Core.Security.Constants;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.EntityConfigurations;

public class OperationClaimConfiguration : IEntityTypeConfiguration<OperationClaim>
{
    public void Configure(EntityTypeBuilder<OperationClaim> builder)
    {
        builder.ToTable("OperationClaims").HasKey(oc => oc.Id);

        builder.Property(oc => oc.Id).HasColumnName("Id").IsRequired();
        builder.Property(oc => oc.Name).HasColumnName("Name").IsRequired();
        builder.Property(oc => oc.CreatedDate).HasColumnName("CreatedDate").IsRequired();
        builder.Property(oc => oc.UpdatedDate).HasColumnName("UpdatedDate");
        builder.Property(oc => oc.DeletedDate).HasColumnName("DeletedDate");

        builder.HasQueryFilter(oc => !oc.DeletedDate.HasValue);

        builder.HasData(GetSeeds());

        builder.HasBaseType((string)null!);
    }

    private List<OperationClaim> GetSeeds()
    {
        return new List<OperationClaim>
        {
            new OperationClaim {
                Id = Guid.Parse("F238078D-892D-41C3-A3B3-AABEA7FBCD23"),
                Name = GeneralOperationClaims.Admin,
                CreatedDate = new DateTime(2024, 11, 17, 18, 52, 42, 67, DateTimeKind.Utc).AddTicks(1507),
            },
            new OperationClaim {
				Id = Guid.Parse("450B017E-CD03-41FE-A9DE-F9B3CD3E534D"),
				Name = GeneralOperationClaims.User,
                CreatedDate = new DateTime(2024, 11, 17, 18, 52, 42, 67, DateTimeKind.Utc).AddTicks(1515),
            },
       };
    }
}
