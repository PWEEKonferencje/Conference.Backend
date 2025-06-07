using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Database.Configuration;

public class ConferenceConfiguration : IEntityTypeConfiguration<Conference>
{
	public void Configure(EntityTypeBuilder<Conference> builder)
	{
		builder.Property(x => x.Name)
			.HasMaxLength(150)
			.IsRequired()
			.IsUnicode();

		builder.Property(x => x.Description)
			.HasMaxLength(1000);
		
		builder.Property(x => x.StartDate)
			.HasColumnType("timestamp without time zone");
		
		builder.Property(x => x.EndDate)
			.HasColumnType("timestamp without time zone");
		
		builder.Property(x => x.ArticlesDeadline)
			.HasColumnType("timestamp without time zone");
		
		builder.Property(x => x.RegistrationDeadline)
			.HasColumnType("timestamp without time zone");

		builder.OwnsOne(x => x.Address);
	}
}