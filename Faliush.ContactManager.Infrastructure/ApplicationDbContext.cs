using Faliush.ContactManager.Infrastructure.Base;
using Faliush.ContactManager.Models;
using Microsoft.EntityFrameworkCore;

namespace Faliush.ContactManager.Infrastructure;

public class ApplicationDbContext : DbContextBase
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)  {  }

    public virtual DbSet<Person> People { get; set; }
    public virtual DbSet<Country> Countries { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Person>().Navigation(x => x.Country).AutoInclude();
    }
}
