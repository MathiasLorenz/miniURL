using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MiniURL.Domain.Entities;

namespace MiniURL.Infrastructure.Persistence.Configurations
{
    public class PersistedURLConfiguration : IEntityTypeConfiguration<PersistedURL>
    {
        public void Configure(EntityTypeBuilder<PersistedURL> builder)
        {
            builder.Property(x => x.UserId)
                .IsRequired(false);

            builder.Property(x => x.URL)
                .IsRequired()
                .HasMaxLength(300);

            builder.Property(x => x.ShortURL)
                .IsRequired()
                .HasMaxLength(8) // I should probably configure this length somewhere...
                .IsFixedLength(true);

            builder.Property(x => x.Deleted)
                .IsRequired();

            builder.HasOne(x => x.User)
                .WithMany(x => x.PersistedURLs)
                .IsRequired(false);
        }
    }
}
