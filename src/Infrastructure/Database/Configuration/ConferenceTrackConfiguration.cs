using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Database.Configuration;

public class ConferenceTrackConfiguration: IEntityTypeConfiguration<ConferenceTrack>
{
    public void Configure(EntityTypeBuilder<ConferenceTrack> builder)
    {
        builder.Property(x => x.Name)
            .HasMaxLength(150)
            .IsRequired()
            .IsUnicode();

    }
}