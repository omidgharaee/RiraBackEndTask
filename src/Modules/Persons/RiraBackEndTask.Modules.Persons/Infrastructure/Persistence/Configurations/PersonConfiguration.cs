using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RiraBackEndTask.Modules.Persons.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace RiraBackEndTask.Modules.Persons.Infrastructure.Persistence.Configurations;

public sealed class PersonConfiguration : IEntityTypeConfiguration<Person>
{
    public void Configure(EntityTypeBuilder<Person> builder)
    {
        builder.ToTable("persons", "persons");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .ValueGeneratedNever();

        builder.Property(x => x.FirstName)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(x => x.LastName)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(x => x.NationalCode)
            .HasMaxLength(10)
            .IsRequired();

        builder.HasIndex(x => x.NationalCode)
            .IsUnique();

        builder.Property(x => x.BirthDate)
            .IsRequired();

        builder.Property(x => x.CreatedAtUtc)
            .IsRequired();
    }
}