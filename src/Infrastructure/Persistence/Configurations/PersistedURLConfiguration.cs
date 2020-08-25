using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MiniURL.Domain.Entities;

namespace MiniURL.Infrastructure.Persistence.Configurations
{
    internal class PersistedURLConfiguration : IEntityTypeConfiguration<PersistedURL>
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
                .HasColumnType("nvarchar(40)") // This was fixed length before and I had to specify this explicitly now for it to work properly.
                .HasMaxLength(40); // The actual length is configured in the appsettings.json file.

            builder.Property(x => x.Deleted)
                .IsRequired();

            builder.HasOne(x => x.User)
                .WithMany(x => x.PersistedURLs)
                .IsRequired(false);
        }
    }
}
