using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Database.Configuration;

public class TrackConfiguration: IEntityTypeConfiguration<Track>
{
    public void Configure(EntityTypeBuilder<Track> builder)
    {
        builder.Property(x => x.Name)
            .HasMaxLength(150)
            .IsRequired()
            .IsUnicode();
        
        builder.Property(x => x.Description)
            .HasMaxLength(1000);
        
    }
}