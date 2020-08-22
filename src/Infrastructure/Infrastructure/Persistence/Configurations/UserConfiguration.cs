using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MiniURL.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MiniURL.Infrastructure.Persistence.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasAlternateKey(x => x.Email);

            builder.Property(x => x.FirstName)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(x => x.LastName)
                .IsRequired()
                .HasMaxLength(50);

            // Todo: Use FluentValidations to validate this on creation.
            builder.Property(x => x.Email)
                .IsRequired()
                .HasMaxLength(100);
        }
    }
}
