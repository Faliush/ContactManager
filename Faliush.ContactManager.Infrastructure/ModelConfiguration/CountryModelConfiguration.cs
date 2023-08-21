using Faliush.ContactManager.Infrastructure.ModelConfiguration.Base;
using Faliush.ContactManager.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Faliush.ContactManager.Infrastructure.ModelConfiguration;

public class CountryModelConfiguration : IdentityModelConfiguration<Country>
{
    protected override void AddBuilder(EntityTypeBuilder<Country> builder)
    {
        builder.Property(x => x.Name).IsRequired().HasMaxLength(40);
        
        builder.HasMany(x => x.People);
    }

    protected override string TableName()
    {
        return "Countries";
    }
}
