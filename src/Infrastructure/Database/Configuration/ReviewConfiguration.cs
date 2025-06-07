using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Database.Configuration;

public class ReviewConfiguration : IEntityTypeConfiguration<Review>
{
	public void Configure(EntityTypeBuilder<Review> builder)
	{
		builder.HasKey(x => x.Id);
		builder.HasOne(x => x.Paper).WithMany(x => x.Reviews);
		builder.HasOne(x => x.Reviewer).WithMany(x => x.Reviews);
		builder.HasMany(x => x.StatusRevisions).WithOne(x => x.Review);
		builder.HasMany(x => x.Revisions).WithOne(x => x.Review);
		builder.Property(x => x.Comment).HasMaxLength(1000);
	}
}