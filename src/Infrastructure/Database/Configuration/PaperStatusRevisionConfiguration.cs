using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Database.Configuration;

public class PaperStatusRevisionConfiguration : IEntityTypeConfiguration<PaperStatusRevision>
{
	public void Configure(EntityTypeBuilder<PaperStatusRevision> builder)
	{
		builder.HasKey(x => x.Id);
		builder.Property(x => x.Timestamp).HasColumnType("datetime without time zone");
		builder.HasOne(x => x.Paper).WithMany(x => x.StatusRevision);
	}
}