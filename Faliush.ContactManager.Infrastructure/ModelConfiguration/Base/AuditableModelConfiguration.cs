using Faliush.ContactManager.Models.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Faliush.ContactManager.Infrastructure.ModelConfiguration.Base;

public abstract class AuditableModelConfiguration<T> : IEntityTypeConfiguration<T> where T : Auditable
{
    public void Configure(EntityTypeBuilder<T> builder)
    {
        builder.ToTable(TableName());
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).IsRequired();

        builder.Property(x => x.CreatedAt).IsRequired().HasConversion(v => v, v => DateTime.SpecifyKind(v, DateTimeKind.Utc));
        builder.Property(x => x.CreatedBy).IsRequired().HasMaxLength(256);
        builder.Property(x => x.UpdatedAt).HasConversion(v => v, v => DateTime.SpecifyKind(v, DateTimeKind.Utc));
        builder.Property(x => x.UpdatedBy).HasMaxLength(256);

        AddBuilder(builder);
    }

    protected abstract void AddBuilder(EntityTypeBuilder<T> builder);

    protected abstract string TableName();
}
