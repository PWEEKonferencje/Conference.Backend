using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Database.Configuration;

public class PaperFileRevisionConfiguration : IEntityTypeConfiguration<PaperFileRevision>
{
	public void Configure(EntityTypeBuilder<PaperFileRevision> builder)
	{
		builder.HasKey(x => x.Id);
		builder.Property(x => x.Timestamp).HasColumnType("timestamp with time zone");
		builder.HasOne(x => x.File);
		builder.HasOne(x => x.Paper).WithMany(x => x.FileRevisions);
	}
}