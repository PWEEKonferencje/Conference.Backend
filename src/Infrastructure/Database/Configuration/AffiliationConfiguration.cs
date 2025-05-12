using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Database.Configuration;

public class AffiliationConfiguration : IEntityTypeConfiguration<Affiliation>
{
	public void Configure(EntityTypeBuilder<Affiliation> builder)
	{
		builder.Property(x => x.Workplace)
			.HasMaxLength(150);
		
		builder.Property(x => x.Position)
			.HasMaxLength(100);
	}
}