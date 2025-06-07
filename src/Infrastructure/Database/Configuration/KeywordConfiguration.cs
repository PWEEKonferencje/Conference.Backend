using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Database.Configuration;

public class KeywordConfiguration : IEntityTypeConfiguration<Keyword>
{
	public void Configure(EntityTypeBuilder<Keyword> builder)
	{
		builder.HasKey(x => x.Id);
		builder.HasOne(x => x.Paper).WithMany(x => x.Keywords);
		builder.Property(x => x.Value).HasMaxLength(50);
	}
}