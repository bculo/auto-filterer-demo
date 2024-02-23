using FilterAPI.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FilterAPI.Persistence.Configurations;

public class PersonConfiguration : IEntityTypeConfiguration<Person>
{
    public void Configure(EntityTypeBuilder<Person> builder)
    {
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.FirstName)
            .HasMaxLength(255)
            .IsRequired();
        
        builder.Property(x => x.LastName)
            .HasMaxLength(255)
            .IsRequired();
    }
}