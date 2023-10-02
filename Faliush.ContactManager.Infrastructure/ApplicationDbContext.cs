using Faliush.ContactManager.Infrastructure.Base;
using Faliush.ContactManager.Models;
using Microsoft.EntityFrameworkCore;

namespace Faliush.ContactManager.Infrastructure;

public class ApplicationDbContext : DbContextBase
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) => 
        base.Database.EnsureCreated();

    public virtual DbSet<Person> People => Set<Person>(); // 
    public virtual DbSet<Country> Countries => Set<Country>(); //

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Person>().Navigation(x => x.Country).AutoInclude();
    }
}
