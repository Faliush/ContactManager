using Faliush.ContactManager.Models.Base;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Faliush.ContactManager.Infrastructure.Base;

public abstract class DbContextBase : DbContext
{
	private const string DefaultUserName = "Anonymous";
	private readonly DateTime DefaultDate = DateTime.UtcNow.ToUniversalTime();
	
	public DbContextBase(DbContextOptions<DbContextBase> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        var applyGenericMethod = typeof(ModelBuilder).GetMethods(BindingFlags.Instance | BindingFlags.Public).First(x => x.Name == "ApplyConfiguration");
        foreach (var type in Assembly.GetExecutingAssembly().GetTypes().Where(c => c.IsClass && !c.IsAbstract && !c.ContainsGenericParameters))
        {
            foreach (var item in type.GetInterfaces())
            {
                if (!item.IsConstructedGenericType || item.GetGenericTypeDefinition() != typeof(IEntityTypeConfiguration<>))
                    continue;
                
                var applyConcreteMethod = applyGenericMethod.MakeGenericMethod(item.GenericTypeArguments[0]);
                applyConcreteMethod.Invoke(modelBuilder, new[] { Activator.CreateInstance(type) });
                break;
            }
        }
    }

    public override int SaveChanges()
    {
        DbSaveChanges();
        return base.SaveChanges();
    }
    public override int SaveChanges(bool acceptAllChangesOnSuccess)
    {
        DbSaveChanges();
        return base.SaveChanges(acceptAllChangesOnSuccess);
    }
    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        DbSaveChanges();
        return base.SaveChangesAsync(cancellationToken);
    }
    public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
    {
        DbSaveChanges();
        return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
    }

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

            entry.Property(nameof(IAudetable.CreatedBy)).CurrentValue = userName;
			entry.Property(nameof(IAudetable.UpdatedBy)).CurrentValue = userName;

            if (createdAt != null)
            {
                if (DateTime.Parse(createdAt.ToString()).Year > 1970)
                    entry.Property(nameof(IAudetable.CreatedAt)).CurrentValue = ((DateTime)createdAt).ToUniversalTime();
                
                else
                    entry.Property(nameof(IAudetable.CreatedAt)).CurrentValue = DefaultDate;
            }
            else
            {
                entry.Property(nameof(IAudetable.CreatedAt)).CurrentValue = DefaultDate;
            }

            if (updatedAt != null)
            {
                if (DateTime.Parse(updatedAt.ToString()).Year > 1970)
                    entry.Property(nameof(IAudetable.UpdatedAt)).CurrentValue = ((DateTime)updatedAt).ToUniversalTime();
                
                else
                    entry.Property(nameof(IAudetable.UpdatedAt)).CurrentValue = DefaultDate;
            }
            else
            {
                entry.Property(nameof(IAudetable.UpdatedAt)).CurrentValue = DefaultDate;
            }
        }

        var modifiedEntries = ChangeTracker.Entries().Where(x => x.State == EntityState.Modified);

        foreach(var entry in modifiedEntries)
        {
            if (entry is not IAudetable)
                continue;

            var userName = entry.Property(nameof(IAudetable.UpdatedBy)).CurrentValue == null
                ? DefaultUserName
                : entry.Property(nameof(IAudetable.UpdatedBy)).CurrentValue;
            var updatedAt = entry.Property(nameof(IAudetable.UpdatedAt)).CurrentValue = DefaultDate;
            var updatedBy = entry.Property(nameof(IAudetable.UpdatedBy)).CurrentValue = userName;
        }
	}
}
