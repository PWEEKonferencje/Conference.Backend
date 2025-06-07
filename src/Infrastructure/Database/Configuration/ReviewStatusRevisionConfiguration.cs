using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Database.Configuration;

public class ReviewStatusRevisionConfiguration : IEntityTypeConfiguration<ReviewStatusRevision>
{
	public void Configure(EntityTypeBuilder<ReviewStatusRevision> builder)
	{
		builder.HasKey(x => x.Id);
		builder.HasOne(x => x.Review).WithMany(x => x.StatusRevisions);
		builder.Property(x => x.Timestamp).HasColumnType("datetime without time zone");
	}
}