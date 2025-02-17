using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Database.Configuration;

public class UserSnapshotConfiguration : IEntityTypeConfiguration<UserSnapshot>
{
	public void Configure(EntityTypeBuilder<UserSnapshot> builder)
	{
		builder.Property(x => x.OrcidId)
			.HasMaxLength(16);
		
		builder.Property(x => x.Name)
			.HasMaxLength(50);
		
		builder.Property(x => x.Surname)
			.HasMaxLength(50);

		builder.Property(x => x.Degree)
			.HasMaxLength(40);
		
		builder.Property(x => x.Workplace)
			.HasMaxLength(150);
		
		builder.Property(x => x.Position)
			.HasMaxLength(100);
	}
}