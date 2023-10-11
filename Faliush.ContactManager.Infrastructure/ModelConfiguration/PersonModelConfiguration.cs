using Faliush.ContactManager.Infrastructure.ModelConfiguration.Base;
using Faliush.ContactManager.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Faliush.ContactManager.Infrastructure.ModelConfiguration;

public class PersonModelConfiguration : AuditableModelConfiguration<Person>
{
    protected override void AddBuilder(EntityTypeBuilder<Person> builder)
    {
        builder.Property(x => x.FirstName).IsRequired().HasMaxLength(30);
        builder.Property(x => x.LastName).IsRequired().HasMaxLength(30);
        builder.Property(x => x.Email).IsRequired();
        builder.Property(x => x.Address).HasMaxLength(80);
        builder.Property(x => x.CountryId);

        builder.HasOne(x => x.Country)
               .WithMany(x => x.People)
               .HasForeignKey(x => x.CountryId)
               .OnDelete(DeleteBehavior.Cascade);
    }

    protected override string TableName()
    {
        return "People";
    }
}
