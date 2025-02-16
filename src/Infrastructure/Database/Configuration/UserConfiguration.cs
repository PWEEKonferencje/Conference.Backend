using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Database.Configuration;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
	public void Configure(EntityTypeBuilder<User> builder)
	{
		builder.Property(x => x.OrcidId)
			.HasMaxLength(16);
		
		builder.Property(x => x.Name)
			.HasMaxLength(50);
		
		builder.Property(x => x.Surname)
			.HasMaxLength(50);

		builder.Property(x => x.Degree)
			.HasMaxLength(40);
	}
}