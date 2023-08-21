using Faliush.ContactManager.Models.Base;
using Microsoft.EntityFrameworkCore;

namespace Faliush.ContactManager.Infrastructure.Base;

public abstract class DbContextBase : DbContext
{
	private const string DefaultUserName = "Anonymous";
	private readonly DateTime DefaultDate = DateTime.UtcNow.ToUniversalTime();
	
	public DbContextBase(DbContextOptions<DbContextBase> options) : base(options) { }

	private void DbSaveChanges()
	{
		var addedEntries = ChangeTracker.Entries().Where(x => x.State == EntityState.Added);

		foreach(var entry in addedEntries)
		{
			if (entry is not IAudetable)
				continue;

            var createdAt = entry.Property(nameof(IAudetable.CreatedAt)).CurrentValue;
			var createdBy = entry.Property(nameof(IAudetable.CreatedBy)).CurrentValue;
			var updatedAt = entry.Property(nameof(IAudetable.UpdatedAt)).CurrentValue; 
			var updatedBy = entry.Property(nameof(IAudetable.UpdatedBy)).CurrentValue;
			var userName = createdBy is null 
                ? DefaultUserName 
                : entry.Property(nameof(IAudetable.CreatedBy)).CurrentValue;

            entry.Property(nameof(IAudetable.CreatedAt)).CurrentValue = userName;
			entry.Property(nameof(IAudetable.UpdatedAt)).CurrentValue = userName;

            if (createdAt != null)
            {
                if (DateTime.Parse(createdAt.ToString()).Year > 1970)
                    entry.Property("CreatedAt").CurrentValue = ((DateTime)createdAt).ToUniversalTime();
                
                else
                    entry.Property("CreatedAt").CurrentValue = DefaultDate;
            }
            else
            {
                entry.Property("CreatedAt").CurrentValue = DefaultDate;
            }

            if (updatedAt != null)
            {
                if (DateTime.Parse(updatedAt.ToString()).Year > 1970)
                    entry.Property("UpdatedAt").CurrentValue = ((DateTime)updatedAt).ToUniversalTime();
                
                else
                    entry.Property("UpdatedAt").CurrentValue = DefaultDate;
            }
            else
            {
                entry.Property("UpdatedAt").CurrentValue = DefaultDate;
            }
        }

        var modifiedEntries = ChangeTracker.Entries().Where(x => x.State == EntityState.Modified);

        foreach(var entry in modifiedEntries)
	}
}
