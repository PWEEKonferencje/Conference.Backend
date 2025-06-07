using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Database.Configuration;

public class ReviewRevisionConfiguration : IEntityTypeConfiguration<ReviewRevision>
{
	public void Configure(EntityTypeBuilder<ReviewRevision> builder)
	{
		builder.HasKey(x => x.Id);
		builder.Property(x => x.PreviousContent).HasMaxLength(1000);
		builder.Property(x => x.CurrentContent).HasMaxLength(1000);
		builder.Property(x => x.Timestamp).HasColumnType("timestamp with time zone");
		builder.HasOne(x => x.Review).WithMany(x => x.Revisions);
	}
}