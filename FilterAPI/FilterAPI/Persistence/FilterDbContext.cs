using FilterAPI.Entities;
using Microsoft.EntityFrameworkCore;

namespace FilterAPI.Persistence;

public class FilterDbContext : DbContext
{
    public virtual DbSet<Person> Data => Set<Person>();
    
    public FilterDbContext(DbContextOptions<FilterDbContext> options) 
        : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(FilterDbContext).Assembly);
    }
}