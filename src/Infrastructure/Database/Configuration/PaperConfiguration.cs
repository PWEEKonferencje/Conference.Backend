using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Database.Configuration;

public class PaperConfiguration : IEntityTypeConfiguration<Paper>
{
	public void Configure(EntityTypeBuilder<Paper> builder)
	{
		builder.HasKey(x => x.Id);
		builder.Property(x => x.Id).ValueGeneratedOnAdd();
		builder.HasOne(x => x.Conference).WithMany(x => x.Papers);
		builder.HasMany(x => x.FileRevisions).WithOne(x => x.Paper);
		builder.HasMany(x => x.StatusRevision).WithOne(x => x.Paper);
		builder.HasOne(x => x.Track).WithMany(x => x.Papers);
		builder.HasOne(x => x.Creator).WithMany(x => x.Papers);
		builder.HasMany(x => x.Keywords).WithOne(x => x.Paper);

		builder.Property(x => x.Abstract).HasMaxLength(300);
		builder.Property(x => x.Title).HasMaxLength(200);
	}
}